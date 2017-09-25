using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Models;
using FourWallpapers.Scrapper.SiteScrappers;
using ImageSharp;
using Image = ImageSharp.Image;

namespace FourWallpapers.Scrapper
{
    //Core Class Build the diff Processors here and then roll thru them

    public class Scraper
    {
        private static readonly Dictionary<string, IScrapeProcessor> Processors =
            new Dictionary<string, IScrapeProcessor>();

        private static IGlobalSettings _globalSettings;
        private static ScrapeRepositories _repos;
        private static readonly Guid ScrapeId = Guid.NewGuid();

        public Scraper(IGlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;

            _repos = new ScrapeRepositories(_globalSettings);

            Helpers.ScrapeRepositories = _repos;

            Processors.Add("4chan", new FourChan(_repos));
            Processors.Add("7chan", new SevenChan(_repos));
            Processors.Add("reddit", new Reddit(_repos));
        }


        public void Run()
        {
            //go thru boards
            foreach (var boardIdentifier in _globalSettings.Scraper.Boards)
                try
                {
                    switch (boardIdentifier)
                    {
                        case Enums.Sources.SlashHR:
                        case Enums.Sources.SlashV:
                        case Enums.Sources.SlashW:
                        case Enums.Sources.SlashWG:
                            Processors["4chan"].Process(boardIdentifier);
                            break;
                        case Enums.Sources.SevenChan:
                            Processors["7chan"].Process(boardIdentifier);
                            break;
                        case Enums.Sources.RedditWallpaper:
                        case Enums.Sources.RedditWallpapers:
                        case Enums.Sources.RedditEarthPorn:
                        case Enums.Sources.RedditMultiwall:
                        case Enums.Sources.Reddit4to3Wallpapers:
                        case Enums.Sources.RedditHiRes:
                        case Enums.Sources.RedditSpacePorn:
                        case Enums.Sources.RedditWQHDWallpaper:
                        case Enums.Sources.RedditWallpaperDump:
                        case Enums.Sources.RedditWidescreenWallpaper:
                            Processors["reddit"].Process(boardIdentifier);
                            break;
                        case Enums.Sources.RedditNsfwWallpapers:
                            Processors["reddit"].Process(boardIdentifier, Enums.Classes.NotSafeForWork);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.LogMessage(
                        $"Process() Error on Board {Enum.GetName(typeof(Enums.Sources), boardIdentifier)}!");
                    Helpers.LogMessage($"Error Message {ex.Message}!");
                }

            Helpers.LogMessage($"Images to Download {_repos.Queue.Count}!");

            //list of all running tasks
            var runningTasks = new List<Task>();

            //create the tasks for downloads
            for (var x = 0; x < 4; x++) runningTasks.Add(Task.Run(() => ProcessImages()));

            //create the task that waits for them to complete
            var completed = Task.WhenAll(runningTasks);

            var iter = 0;

            //loop with a sleep 
            while (!completed.IsCompleted)
            {
                if (iter % 10 == 0) Helpers.LogMessage($"waiting ({_repos.Queue.Count}) ");
                iter++;
                Thread.Sleep(250);
            }
        }

        /// <summary>
        ///     Gets contents of the image
        /// </summary>
        /// <param name="url"></param>
        private static byte[] GetImageContents(string url)
        {
            try
            {
                return _repos.Client.GetByteArrayAsync(url).Result;
            }
            catch (Exception ex)
            {
                Helpers.LogMessage($"GetImageContents() Error on {url}!");
                Helpers.LogMessage($"Error Message {ex.Message}!");
                return null;
            }
        }


        private static void ProcessImages()
        {
            while (_repos.Queue.Count > 0)
            {
                var download = _repos.Queue.Dequeue();
                if (download == null) continue;
                using (var sha512 = SHA512.Create())
                {
                    using (var md5 = MD5.Create())
                    {
                        //Helpers.LogMessage($"Starting Image : {download.ImageId}");
                        if (download.ImageExtension == "webm") continue;
                        try
                        {
                            var image = GetImageContents(download.ImageUrl.StartsWith("http")
                                ? download.ImageUrl
                                : "http:" + download.ImageUrl);

                            if (image == null) continue;

                            download.Hash = Core.Helpers.ByteToString(sha512.ComputeHash(image));

                            //Helpers.LogMessage($"{download.ImageId} : Processing Image");

                            if (_repos.ImageScrapeRepository.HashExistsAsync(download.Hash, CancellationToken.None)
                                .GetAwaiter().GetResult())
                            {
                                //hash already exists
                                //Helpers.LogMessage($"{download.ImageId} : O Crap Duplicate");
                                //add to scrape repo
                                if (!_repos.ImageScrapeRepository
                                    .ExistsAsync(download.ImageId, download.IndexSource, CancellationToken.None)
                                    .GetAwaiter().GetResult())
                                    _repos.ImageScrapeRepository.AddAsync(new ImageScrape
                                    {
                                        ImageId = download.ImageId,
                                        Source = download.IndexSource,
                                        Hash = download.Hash,
                                        ScrapeId = ScrapeId
                                    }, CancellationToken.None).GetAwaiter().GetResult();
                                continue;
                            }

                            var sanitizedId = Core.Helpers.ByteToString(md5.ComputeHash(image));

                            var filename = $"{sanitizedId}.{download.ImageExtension}";

                            //if hash doesnt exists
                            if (!File.Exists($"{_globalSettings.Scraper.ImageLocation}{filename}"))
                                File.WriteAllBytes($"{_globalSettings.Scraper.ImageLocation}{filename}",
                                    image);

                            if (!File.Exists($"{_globalSettings.Scraper.ThumbnailLocation}{filename}") ||
                                string.IsNullOrWhiteSpace(download.Resolution))
                                using (var imageData = new Image(image))
                                {
                                    if (string.IsNullOrWhiteSpace(download.Resolution))
                                        download.Resolution = $"{imageData.Width}x{imageData.Height}";

                                    if (imageData.Height < 720 || imageData.Width < 1024)
                                    {
                                        Helpers.LogMessage($"Image is too Small: {imageData.Width}x{imageData.Height}");
                                        _repos.ImageScrapeRepository.AddAsync(new ImageScrape
                                        {
                                            ImageId = download.ImageId,
                                            Source = download.IndexSource,
                                            Hash = download.Hash,
                                            ScrapeId = ScrapeId
                                        }, CancellationToken.None);
                                        continue;
                                    }

                                    if (!File.Exists(
                                        $"{_globalSettings.Scraper.ThumbnailLocation}{filename}"))
                                    {
                                        int newHeight, newWidth;
                                        try
                                        {
                                            if (imageData.Height > imageData.Width)
                                            {
                                                newHeight = Constants.ThumbnailSize;
                                                newWidth = imageData.Width /
                                                           (imageData.Height / Constants.ThumbnailSize);
                                            }
                                            else
                                            {
                                                newWidth = Constants.ThumbnailSize;
                                                newHeight = imageData.Height /
                                                            (imageData.Width / Constants.ThumbnailSize);
                                            }
                                        }
                                        catch
                                        {
                                            Helpers.LogMessage($"Thumbnail attempted to divide by zero?");
                                            newWidth = Constants.ThumbnailSize;
                                            newHeight = Constants.ThumbnailSize;
                                        }

                                        //thumbnail doesnt exists make it
                                        imageData.Resize(newWidth, newHeight);
                                        imageData.Save(
                                            $"{_globalSettings.Scraper.ThumbnailLocation}{filename}");
                                        imageData.Dispose();
                                        //Helpers.LogMessage($"{download.ImageId} :: Saved Thumbnail!");
                                    }
                                }


                            //add to scrape repo
                            _repos.ImageScrapeRepository.AddAsync(new ImageScrape
                            {
                                ImageId = download.ImageId,
                                Source = download.IndexSource,
                                Hash = download.Hash,
                                ScrapeId = ScrapeId
                            }, CancellationToken.None);
                            //Helpers.LogMessage($"{download.ImageId} :: Marked as Scraped!");

                            //build new image
                            var newImage = new Models.Image
                            {
                                ImageId = sanitizedId,
                                Class = download.Class,
                                IndexSource = download.IndexSource,
                                Who = download.Who,
                                Tripcode = download.TripCode,
                                ResolutionX = int.Parse(download.Resolution.Split('x').First()),
                                ResolutionY = int.Parse(download.Resolution.Split('x').Last()),
                                TagsString = download.Tag,
                                DateDownloaded = DateTime.UtcNow,
                                Reported = Enums.Reported.Unreported,
                                Hash = download.Hash,
                                FileExtension = download.ImageUrl.Split('.').Last(),
                                ServerId = _globalSettings.Scraper.ScrapeServer,
                                Size = image.Length / 1024m
                            };

                            //calculate ratio
                            var r = Core.Helpers.RatioCalculate(newImage.ResolutionX, newImage.ResolutionY);


                            try
                            {
                                newImage.Ratio = $"{newImage.ResolutionX / r}:{newImage.ResolutionY / r}";
                            }
                            catch
                            {
                                Helpers.LogMessage($"Ratio attempted to divide by zero?");
                            }

                            //add to imageRepo?
                            _repos.ImageRepository.AddAsync(newImage, CancellationToken.None);
                            //Helpers.LogMessage($"{download.ImageId} :: Added to Repo!");
                        }
                        catch (Exception ex)
                        {
                            Helpers.LogMessage($"Processing Image {download.ImageId} : Error {ex.Message}!");
                        }
                    }
                }
            }
        }
    }
}