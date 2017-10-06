using System.Collections.Generic;

namespace FourWallpapers.Core
{
    public static class Constants
    {
        public static Dictionary<Enums.Sources, string> SourceUrls = new Dictionary<Enums.Sources, string>
        {
            {Enums.Sources.SlashW, "http://boards.4chan.org/w/"},
            {Enums.Sources.SlashWG, "http://boards.4chan.org/wg/"},
            {Enums.Sources.SlashHR, "http://boards.4chan.org/hr/"},
            {Enums.Sources.SlashV, "http://boards.4chan.org/v/"},

            {Enums.Sources.SevenChan, "https://www.7chan.org/wp/"},

            {Enums.Sources.RedditWallpaper, "https://www.reddit.com/r/wallpaper/"},
            {Enums.Sources.RedditWallpapers, "https://www.reddit.com/r/wallpapers/"},
            {Enums.Sources.RedditNsfwWallpapers, "https://www.reddit.com/r/NSFW_Wallpapers/"},
            {Enums.Sources.RedditMultiwall, "https://www.reddit.com/r/multiwall/"},
            {Enums.Sources.RedditEarthPorn, "https://www.reddit.com/r/EarthPorn/"},
            {Enums.Sources.RedditNoContextWallPapers, "https://www.reddit.com/r/nocontext_wallpapers/"},
            {Enums.Sources.RedditSpacePorn, "https://www.reddit.com/r/spaceporn/"},
            {Enums.Sources.RedditHiRes, "https://www.reddit.com/r/HI_Res/"},
            {Enums.Sources.RedditWidescreenWallpaper, "https://www.reddit.com/r/WidescreenWallpaper/"},
            {Enums.Sources.RedditWQHDWallpaper, "https://www.reddit.com/r/WQHD_Wallpaper/"},
            {Enums.Sources.Reddit4to3Wallpapers, "https://www.reddit.com/r/4to3Wallpapers/"},
            {Enums.Sources.RedditWallpaperDump, "https://www.reddit.com/r/wallpaperdump/"},

            {Enums.Sources.SlashW8Chan, "https://8ch.net/w/"},
            {Enums.Sources.SlashWG8Chan, "https://8ch.net/wg/"},

            {Enums.Sources.Any, ""}
        };
    }
}