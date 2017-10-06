using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using FourWallpapers.Core;

namespace FourWallpapers.Scrapper
{
    public static class Constants
    {
        public const int ThumbnailSize = 250;

        public static Dictionary<Enums.Sources, string> ThreadUrlCleanup = new Dictionary<Enums.Sources, string>
        {
            {Enums.Sources.SlashW, ""},
            {Enums.Sources.SlashWG, ""},
            {Enums.Sources.SlashHR, ""},
            {Enums.Sources.SevenChan, "/wp/"},
            {Enums.Sources.SlashV, ""},
            {Enums.Sources.SlashW8Chan, "/w/"},
            {Enums.Sources.SlashWG8Chan, "/wg/"},
            {Enums.Sources.Any, ""}
        };
    }

    public static class Helpers
    {
        public static ScrapeRepositories ScrapeRepositories = null;

        public static void LogMessage(string messge, int indent = 0)
        {
            Console.WriteLine($"{DateTime.UtcNow} :: {messge.PadLeft(indent, '\t')}");
        }

        /// <summary>
        ///     Gets contents of the page
        /// </summary>
        /// <param name="url"></param>
        public static string GetPageContents(string url)
        {
            try
            {
                return ScrapeRepositories.Client.GetStringAsync(url).Result;
            }
            catch (Exception ex)
            {
                LogMessage($"GetPageContents() Error on {url}!");
                LogMessage($"Error Message {ex.Message}!");
                return null;
            }
        }

        public static ImageDetail ProcessImageElement(this IScrapeProcessor processor, IElement element,
            Enums.Sources source, Enums.Classes classification = Enums.Classes.Any)
        {
            var image = new ImageDetail
            {
                IndexSource = source
            };

            if (classification != Enums.Classes.Any) image.Class = classification;


            if (!processor.GetImageUrl(element, image)) return null;

            processor.GetWhoStamp(element, image);

            processor.GetResolution(element, image);

            processor.GetTags(element, image);

            try
            {
                image.ImageId = image.ImageUrl?.Split('/')?.Last()?.Split('.')?.First() ?? "";
                image.ImageExtension = image.ImageUrl?.Split('/')?.Last()?.Split('.')?.Last() ?? "";
            }
            catch
            {
                // pass
            }

            return image;
        }
    }
}