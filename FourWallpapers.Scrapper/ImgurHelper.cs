using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AngleSharp.Parser.Html;
using FourWallpapers.Core;

// ReSharper disable InconsistentNaming

namespace FourWallpapers.Scrapper
{
    public static class ImgurHelper
    {
        public static void ProcessImgurGallery(string url, Enums.Sources source,
            Enums.Classes classification = Enums.Classes.Any)
        {
            if (string.IsNullOrWhiteSpace(url)) return;
            var page = Helpers.GetPageContents(url);
            var parser = new HtmlParser();
            var document = parser.Parse(page);
            var jsElements = document.GetElementsByTagName("script");

            if (!jsElements.Any())
            {
                Helpers.LogMessage($"Cant find Script Sections for this page {url}");
                return;
            }
            var imagesElement = jsElements.First(element => element.InnerHtml.Contains("window.runSlots")).TextContent;

            var startPosi = imagesElement.IndexOf("{", StringComparison.OrdinalIgnoreCase);
            var firstSemi = imagesElement.IndexOf(";", StringComparison.OrdinalIgnoreCase);
            var stopLength = firstSemi - startPosi;

            var json = imagesElement.Substring(startPosi, stopLength);
            try
            {
                if (string.IsNullOrWhiteSpace(json)) return;
                var images = Core.Helpers.Json.LoadJson<ImgurJson>(json);

                if (images.Item.album_images.images.Any())
                {
                    //item.album_images.images
                    foreach (var image in images.Item.album_images.images)
                        try
                        {
                            //{
                            //    "hash": "kiylMhV",
                            //    "title": "",
                            //    "description": "Verona",
                            //    "width": 1920,
                            //    "height": 1080,
                            //    "size": 484650,
                            //    "ext": ".jpg",
                            //    "animated": false,
                            //    "prefer_video": false,
                            //    "looping": false,
                            //    "datetime": "2017-08-29 22:37:18"

                            //}

                            var title = image.title ?? "";
                            var description = image.description ?? "";
                            var extension = image.ext ?? "";
                            var imageDetail = new ImageDetail
                            {
                                ImageUrl = $"http://i.imgur.com/{image.hash}{image.ext}",
                                ImageId = image.hash,
                                ImageExtension = extension.Remove(0, 1),
                                Resolution = $"{image.width}x{image.height}",
                                Tag = document.GetElementsByClassName("post-title").First().TextContent +
                                      (!string.IsNullOrWhiteSpace(description) ? $",{description}" : "") +
                                      (!string.IsNullOrWhiteSpace(title) ? $",{title}" : ""),
                                Who = images.Item.author ?? "Anonymous",
                                IndexSource = Enums.Sources.Imgur,
                                Source = page
                            };

                            if (classification != Enums.Classes.Any) imageDetail.Class = classification;

                            // - - - - check if images have been scrapped before
                            if (Helpers.ScrapeRepositories.ImageScrapeRepository.ExistsAsync(imageDetail.ImageId,
                                imageDetail.IndexSource, CancellationToken.None).GetAwaiter().GetResult()) continue;

                            //Helpers.LogMessage($"Image not in Scrape Repo {imageDetail.ImageId}");

                            Helpers.ScrapeRepositories.Queue.Enqueue(imageDetail);
                        }
                        catch (Exception e)
                        {
                            Helpers.LogMessage($"Error on Imgur For Loop : {e.Message}");
                        }
                }
                else
                {
                    //singular image
                    var image = images.Item;
                    try
                    {
                        //{
                        //    "hash": "kiylMhV",
                        //    "title": "",
                        //    "description": "Verona",
                        //    "width": 1920,
                        //    "height": 1080,
                        //    "size": 484650,
                        //    "ext": ".jpg",
                        //    "animated": false,
                        //    "prefer_video": false,
                        //    "looping": false,
                        //    "datetime": "2017-08-29 22:37:18"

                        //}

                        var title = image.title ?? "";
                        var description = image.description ?? "";
                        var extension = image.ext ?? "";
                        var imageDetail = new ImageDetail
                        {
                            ImageUrl = $"http://i.imgur.com/{image.hash}{image.ext}",
                            ImageId = image.hash,
                            ImageExtension = extension.Remove(0, 1),
                            Resolution = $"{image.width}x{image.height}",
                            Tag = document.GetElementsByClassName("post-title").First().TextContent +
                                  (!string.IsNullOrWhiteSpace(description) ? $",{description}" : "") +
                                  (!string.IsNullOrWhiteSpace(title) ? $",{title}" : ""),
                            Who = images.Item.author ?? "Anonymous",
                            IndexSource = Enums.Sources.Imgur
                        };

                        if (classification != Enums.Classes.Any) imageDetail.Class = classification;

                        // - - - - check if images have been scrapped before
                        if (Helpers.ScrapeRepositories.ImageScrapeRepository.ExistsAsync(imageDetail.ImageId,
                            imageDetail.IndexSource, CancellationToken.None).GetAwaiter().GetResult()) return;

                        //Helpers.LogMessage($"Image not in Scrape Repo {imageDetail.ImageId}");
                        Helpers.ScrapeRepositories.Queue.Enqueue(imageDetail);
                    }
                    catch (Exception e)
                    {
                        Helpers.LogMessage($"Error on Imgur For Loop : {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                //possibly only a single picture page


                Helpers.LogMessage($"Error on Imgur JSON Parse : {e.Message}");
            }
        }
    }


    public class ImgurJson
    {
        public ImgurJsonItem Item { get; set; } = new ImgurJsonItem();
    }

    public class ImgurJsonItem
    {
        public string author { get; set; } = "";

        public string hash { get; set; }
        public string title { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string description { get; set; }
        public string ext { get; set; }

        public ImgurJsonItemAlbum album_images { get; set; } = new ImgurJsonItemAlbum();
    }

    public class ImgurJsonItemAlbum
    {
        public List<ImgurJsonItemAlbumImage> images { get; set; } = new List<ImgurJsonItemAlbumImage>();
    }

    public class ImgurJsonItemAlbumImage
    {
        public string hash { get; set; }
        public string title { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string description { get; set; }
        public string ext { get; set; }
    }
}