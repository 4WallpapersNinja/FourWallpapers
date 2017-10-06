using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FourWallpapers.Core;
using FourWallpapers.Core.Database.Entities;
using FourWallpapers.Scrapper.SiteScrappers;
using NUglify.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

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
            Processors.Add("8chan", new EightChan(_repos));
            Processors.Add("reddit", new Reddit(_repos));
        }


        public void Run()
        {
            //go thru boards
            var runningTasks = new List<Task>();
            foreach (var boardIdentifier in _globalSettings.Scraper.Boards)
                try
                {
                    switch (boardIdentifier)
                    {
                        case Enums.Sources.SlashHR:
                        case Enums.Sources.SlashV:
                        case Enums.Sources.SlashW:
                        case Enums.Sources.SlashWG:
                            runningTasks.Add(Task.Run(() => Processors["4chan"].Process(boardIdentifier)));
                            
                            break;
                        case Enums.Sources.SevenChan:
                            runningTasks.Add(Task.Run(() => Processors["7chan"].Process(boardIdentifier)));
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
                            runningTasks.Add(Task.Run(() => Processors["reddit"].Process(boardIdentifier)));
                            break;
                        case Enums.Sources.RedditNsfwWallpapers:
                            runningTasks.Add(Task.Run(() => Processors["reddit"].Process(boardIdentifier, Enums.Classes.NotSafeForWork)));
                            break;

                        case Enums.Sources.SlashW8Chan:
                        case Enums.Sources.SlashWG8Chan:
                            runningTasks.Add(Task.Run(() => Processors["8chan"].Process(boardIdentifier)));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.LogMessage(
                        $"Process() Error on Board {Enum.GetName(typeof(Enums.Sources), boardIdentifier)}!");
                    Helpers.LogMessage($"Error Message {ex.Message}!");
                }

            // we run each crawler on it own thread then wait for all to finish
            var iter = 0;
            var completed = Task.WhenAll(runningTasks);
            while (!completed.IsCompleted)
            {
                if (iter % 10 == 0) Helpers.LogMessage($"waiting for scraping to complete!");
                iter++;
                Thread.Sleep(250);
            }

            // we only want distinct urls that are not null
            _repos.Queue = new Queue<ImageDetail>(_repos.Queue.Where(i => i != null).DistinctBy(i => i.ImageUrl));

            Helpers.LogMessage($"Images to Download {_repos.Queue.Count}!");

            //list of all running tasks
            runningTasks.Clear();
            var threadCount = _repos.Queue.Count > 6 ? 6 : _repos.Queue.Count;

            //create the tasks for downloads
            for (var x = 0; x < threadCount; x++) runningTasks.Add(Task.Run(() => ProcessImages()));

            //create the task that waits for them to complete
            completed = Task.WhenAll(runningTasks);


            iter = 0;
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
                return _repos.Client.GetByteArrayAsync(url).GetAwaiter().GetResult();
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
                try
                {
                    var download = _repos.Queue.Dequeue();
                
                    if (download == null) continue;
                    using (var sha512 = SHA512.Create())
                    {
                        using (var md5 = MD5.Create())
                        {
                            Helpers.LogMessage($"Starting Image : {download.ImageId}");
                            if (download.ImageExtension == "webm") continue;
                            try
                            {
                                var image = GetImageContents(download.ImageUrl.StartsWith("http")
                                    ? download.ImageUrl
                                    : "http:" + download.ImageUrl);

                                if (image == null) continue;

                                download.Hash = Core.Helpers.Utilities.ByteToString(sha512.ComputeHash(image));

                                Helpers.LogMessage($"{download.ImageId} : Processing Image");

                                if (_repos.ImageScrapeRepository.HashExistsAsync(download.Hash, CancellationToken.None)
                                    .GetAwaiter().GetResult())
                                {
                                    //hash already exists
                                    Helpers.LogMessage($"{download.ImageId} : O Crap Duplicate");
                                    //add to scrape repo
                                    try
                                    {
                                        _repos.ImageScrapeRepository.AddAsync(new ImageScrape
                                        {
                                            ImageId = download.ImageId,
                                            Source = download.IndexSource,
                                            Hash = download.Hash,
                                            ScrapeId = ScrapeId,
                                            Datestamp = DateTime.UtcNow
                                        }, CancellationToken.None).GetAwaiter().GetResult();
                                    }
                                    catch (Exception ex)
                                    {
                                        Helpers.LogMessage($"Error Adding Dupe to Repo {download.ImageId} : Error {ex.Message}!");
                                    }
                                       
                                    continue;
                                }


                                // add to scrape repo . incase the image fails it still marks it as scrapped and continues. this is for the issues with ImageSharp and memory management

                                _repos.ImageScrapeRepository.AddAsync(new ImageScrape
                                {
                                    ImageId = download.ImageId,
                                    Source = download.IndexSource,
                                    Hash = download.Hash,
                                    ScrapeId = ScrapeId,
                                    Datestamp = DateTime.UtcNow
                                }, CancellationToken.None).GetAwaiter().GetResult();

                                var sanitizedId = Core.Helpers.Utilities.ByteToString(md5.ComputeHash(image));

                                var filename = $"{sanitizedId}.{download.ImageExtension}";

                                System.IO.Directory.CreateDirectory(
                                    $"{_globalSettings.Scraper.ImageLocation}{filename.Substring(0, 2)}/");

                                //if hash doesnt exists
                                if (!File.Exists(
                                    $"{_globalSettings.Scraper.ImageLocation}{filename.Substring(0, 2)}/{filename}"))
                                    File.WriteAllBytes(
                                        $"{_globalSettings.Scraper.ImageLocation}{filename.Substring(0, 2)}/{filename}",
                                        image);

                                if (!File.Exists($"{_globalSettings.Scraper.ThumbnailLocation}{filename}") ||
                                    string.IsNullOrWhiteSpace(download.Resolution))
                                {
                                    using (Image<Rgba32> imageData = SixLabors.ImageSharp.Image.Load(image))
                                    {
                                        if (string.IsNullOrWhiteSpace(download.Resolution))
                                            download.Resolution = $"{imageData.Width}x{imageData.Height}";

                                        if (imageData.Height < 720 || imageData.Width < 1024)
                                        {
                                            Helpers.LogMessage(
                                                $"Image is too Small: {imageData.Width}x{imageData.Height}");
                                            _repos.ImageScrapeRepository.AddAsync(new ImageScrape
                                            {
                                                ImageId = download.ImageId,
                                                Source = download.IndexSource,
                                                Hash = download.Hash,
                                                ScrapeId = ScrapeId,
                                                Datestamp = DateTime.UtcNow
                                            }, CancellationToken.None).GetAwaiter().GetResult();
                                            continue;
                                        }

                                        if (!File.Exists(
                                            $"{_globalSettings.Scraper.ThumbnailLocation}{filename.Substring(0, 2)}/{filename}")
                                        )
                                        {
                                            using (Image<Rgba32> thumbnailData = ResizeImageToThumbnail(imageData))
                                            {
                                                System.IO.Directory.CreateDirectory(
                                                    $"{_globalSettings.Scraper.ThumbnailLocation}{filename.Substring(0, 2)}/");
                                                thumbnailData.Save(
                                                    $"{_globalSettings.Scraper.ThumbnailLocation}{filename.Substring(0, 2)}/{filename}");
                                            }
                                        }
                                    }
                                }


                                Helpers.LogMessage($"{download.ImageId} :: Marked as Scraped!");

                                //build new image
                                var newImage = new Core.Database.Entities.Image
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
                                var r = Core.Helpers.Utilities.RatioCalculate(newImage.ResolutionX,
                                    newImage.ResolutionY);


                                try
                                {
                                    newImage.Ratio = $"{newImage.ResolutionX / r}:{newImage.ResolutionY / r}";
                                }
                                catch
                                {
                                    Helpers.LogMessage($"Ratio attempted to divide by zero?");
                                }

                                //add to imageRepo?
                                _repos.ImageRepository.AddAsync(newImage, CancellationToken.None).GetAwaiter()
                                    .GetResult();
                                Helpers.LogMessage($"{download.ImageId} :: Added to Repo!");
                            }
                            catch (Exception ex)
                            {
                                Helpers.LogMessage($"Processing Image {download.ImageId} : Error {ex.Message}!");
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Helpers.LogMessage($"While Loop Failure : Error {ex.Message}!");
                }
            }
        }

        private static Image<Rgba32> ResizeImageToThumbnail(Image<Rgba32> imageData)
        {
            return imageData.Clone(x => x.Resize(new ResizeOptions
            {
                Size = new Size(Constants.ThumbnailSize, Constants.ThumbnailSize),
                Mode = ResizeMode.Max
            }));
        }
    }
}