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
    public class EightChan : BaseScraper, IScrapeProcessor
    {
        public EightChan(ScrapeRepositories scrapeRepositories) : base(scrapeRepositories)
        {
        }
        
        public void Process(Enums.Sources source, Enums.Classes classification = Enums.Classes.Any)
        {
            try
            {
                //Get the board URL.
                var board = Core.Constants.SourceUrls[source];

                Helpers.LogMessage($"Starting Board: {board}");

                //loop pages
                for (var x = 1; x <= Settings.MaxPages; x++)
                {
                    var url = x == 1 ? "index.html" : $"{x}";

                    //Helpers.LogMessage($"Getting Page Contents for {url}");
                    var pageContents = Helpers.GetPageContents(board + url);


                    // - - get threads on pages
                    //Helpers.LogMessage($"Getting Page Threads");
                    var threads = GetPageThreads(pageContents);
                    //Helpers.LogMessage($"Threads found :: {threads.Count}");

                    //loop the threads on the this page
                    foreach (var thread in threads)
                    {
                        //generate thread url
                        var threadUrl = !string.IsNullOrWhiteSpace(Constants.ThreadUrlCleanup[source])
                            ? thread.Replace(Constants.ThreadUrlCleanup[source], "")
                            : thread;

                        //Helpers.LogMessage($"Getting Thread Contents :: {threadUrl}");
                        var threadContents = Helpers.GetPageContents(board + threadUrl);

                        //get post + all replys

                        //Helpers.LogMessage($"Processing Post");
                        var posts = GetPagePosts(threadContents);

                        foreach (var post in posts)
                        {
                            //Helpers.LogMessage($"Processing Images");
                            var image = this.ProcessImageElement(post, source);

                            image.Source = threadUrl;

                            if (string.IsNullOrWhiteSpace(image?.ImageId)) continue;

                            // - - - - check if images have been scrapped before
                            if (ScrapeRepositories.ImageScrapeRepository
                                .ExistsAsync(image.ImageId, image.IndexSource, CancellationToken.None).GetAwaiter()
                                .GetResult()) continue;


                            Helpers.LogMessage($"Image not in Scrape Repo {image.ImageId}");

                            ScrapeRepositories.Queue.Enqueue(image);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public bool GetImageUrl(IElement element, ImageDetail image)
        {
            try
            {
                var imageUrlElement = element.GetElementsByClassName("fileinfo");

                image.ImageUrl = imageUrlElement
                    .SelectMany(e => e.GetElementsByTagName("a")).First(e => !e.HasAttribute("class")).GetAttribute("href");



                if (string.IsNullOrWhiteSpace(image.ImageUrl)) return false;

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
                var nameElement = element.GetElementsByClassName("name");

                image.Who = nameElement.First().InnerHtml ?? "Anonymous";
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
                var fileElement = element.GetElementsByClassName("unimportant");


                //get its text
                var fileString = fileElement.First().TextContent ?? "";

                var match = Regex.Match(fileString, @"(\d+)x(\d+)", RegexOptions.IgnoreCase);

                if (float.Parse(match.Groups[1].Value) < 1024 || float.Parse(match.Groups[2].Value) < 720)
                {
                    return false;
                }

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
                var fileElement = element.GetElementsByClassName("fileText");

                //get its text
                var fileString = fileElement.First().TextContent ?? "";

                image.Tag = fileString;
                if (Regex.Match(fileString, @"\d+\.(png|jpg|gif)", RegexOptions.IgnoreCase).Success) image.Tag = "";
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets all the "threads" / "posts" on the page provided
        /// </summary>
        private List<string> GetPageThreads(string page)
        {
            try
            {
                var parser = new HtmlParser();
                var document = parser.Parse(page);
                var output = new List<string>();

                //4chan threads
                output.AddRange(document.GetElementsByClassName("thread")
                    .SelectMany(thread => thread.GetElementsByClassName("post"))
                    .SelectMany(post => post.GetElementsByClassName("intro"))
                    .SelectMany(intro => intro.GetElementsByTagName("a").Where(i=> !i.HasAttribute("class")))
                    .Select(link => link.GetAttribute("href")).ToList());

                return output;
            }
            catch (Exception ex)
            {
                Helpers.LogMessage($"GetPageThreads() : Error {ex.Message}");
                throw;
            }
        }
        
        private List<IElement> GetPagePosts(string page)
        {
            try
            {
                var parser = new HtmlParser();
                var document = parser.Parse(page);
                var elements = document.GetElementsByClassName("files").ToList();
                return elements;
            }
            catch (Exception ex)
            {
                Helpers.LogMessage($"GetPagePost() : Error {ex.Message}");
                throw;
            }
        }
    }
}