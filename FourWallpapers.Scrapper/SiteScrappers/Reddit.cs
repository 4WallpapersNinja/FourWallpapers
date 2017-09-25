using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using FourWallpapers.Core;

namespace FourWallpapers.Scrapper.SiteScrappers
{
    public class Reddit : BaseScraper, IScrapeProcessor
    {
        public Reddit(ScrapeRepositories scrapeRepositories) : base(scrapeRepositories)
        {
        }

        public bool GetImageUrl(IElement element, ImageDetail image)
        {
            try
            {
                image.ImageUrl = element.GetAttribute("data-url");

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetWhoStamp(IElement element, ImageDetail image)
        {
            try
            {
                image.Who = element.GetAttribute("data-author");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetResolution(IElement element, ImageDetail image)
        {
            //get resolution
            try
            {
                //get the file element
                var fileString = element.GetElementsByClassName("entry ")
                    .SelectMany(ele => ele.GetElementsByClassName("top-matter"))
                    .SelectMany(ele => ele.GetElementsByClassName("title")).First().TextContent;

                var match = Regex.Match(fileString, @"(\d+)x(\d+)", RegexOptions.IgnoreCase);

                image.Resolution = match.ToString();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GetTags(IElement element, ImageDetail image)
        {
            try
            {
                //get the file element
                var fileString = element.GetElementsByClassName("entry ")
                    .SelectMany(ele => ele.GetElementsByClassName("top-matter"))
                    .SelectMany(ele => ele.GetElementsByClassName("title")).First().TextContent;

                image.Tag = fileString;
                if (Regex.Match(fileString, @"\d+\.(png|jpg|gif)", RegexOptions.IgnoreCase).Success) image.Tag = "";
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Process(Enums.Sources source, Enums.Classes classification = Enums.Classes.Any)
        {
            var url = "";

            try
            {
                //Get the board URL.
                var board = Core.Constants.SourceUrls[source];
                Helpers.LogMessage($"Starting Board: {board}");

                var tabs = new List<string> {"top", "new", ""};

                foreach (var tab in tabs)
                {
                    var boardurl = $"{board}{tab}";
                    var lastPost = "";
                    //loop pages
                    for (var x = 1; x <= Settings.MaxPages; x++)
                    {
                        if (x != 1) url = $"?count={(x - 1) * 25}&after={lastPost}";

                        Helpers.LogMessage($"Getting Page Contents for {boardurl}{url}");
                        var pageContents = Helpers.GetPageContents(boardurl + url);


                        // - - get threads on pages
                        //Helpers.LogMessage($"Getting Page Threads");
                        var threads = GetPageThreads(pageContents);
                        //Helpers.LogMessage($"Threads found :: {threads.Count}");

                        if (!threads.Any()) break;
                        //loop the threads on the this page
                        foreach (var thread in threads)
                        {
                            var imagelink = thread.GetAttribute("data-domain");
                            var dataUrl = thread.GetAttribute("data-url");

                            try
                            {
                                if (imagelink == "imgur.com")
                                {
                                    var ext = dataUrl.Split("/").Last().Split(".").Last();
                                    if (new[] {"png", "jpeg", "jpg", "gif", "png"}.Contains(ext))
                                        imagelink = "i.imgur.com";
                                }
                            }
                            catch
                            {
                                //do nothing
                            }

                            //determine the action to run
                            switch (imagelink)
                            {
                                case "i.redd.it":
                                //if domain = i.redd.it this is a direct link
                                case "i.imgur.com":
                                    //if domain = i.imgur.com this is a direct link
                                    var image = new ImageDetail
                                    {
                                        IndexSource = source
                                    };

                                    if (classification != Enums.Classes.Any) image.Class = classification;

                                    if (!GetImageUrl(thread, image)) continue;

                                    GetWhoStamp(thread, image);

                                    GetResolution(thread, image);

                                    GetTags(thread, image);

                                    try
                                    {
                                        image.ImageId = thread.GetAttribute("data-fullname");
                                        image.ImageExtension =
                                            image.ImageUrl?.Split('/')?.Last()?.Split('.')?.Last() ?? "";
                                    }
                                    catch
                                    {
                                        // pass
                                    }

                                    // - - - - check if images have been scrapped before
                                    if (ScrapeRepositories.ImageScrapeRepository.ExistsAsync(image.ImageId,
                                        imagelink == "i.imgur.com" ? Enums.Sources.Imgur : source,
                                        CancellationToken.None).GetAwaiter().GetResult()) continue;


                                    //Helpers.LogMessage($"Image not in Scrape Repo {image.ImageId}");

                                    if (image != null)
                                        ScrapeRepositories.Queue.Enqueue(image);

                                    break;
                                case "imgur.com":
                                    //if domain = imgur.com this is a gallery gotta go loop this shit
                                    ImgurHelper.ProcessImgurGallery(thread.GetAttribute("data-url") ?? "", source,
                                        classification);
                                    break;
                            }
                        }

                        if (threads.Any()) lastPost = threads.Last().GetAttribute("data-fullname");
                        else break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private List<IElement> GetPageThreads(string page)
        {
            try
            {
                var parser = new HtmlParser();
                var document = parser.Parse(page);

                //reddit threads
                return document.GetElementsByClassName("linklisting").First()
                    .GetElementsByClassName("thing").ToList();
            }
            catch (Exception ex)
            {
                Helpers.LogMessage($"GetPageThreads() : Error {ex.Message}");
                throw;
            }
        }
    }
}