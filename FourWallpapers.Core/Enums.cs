using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// ReSharper disable InconsistentNaming

namespace FourWallpapers.Core
{
    public static class Enums
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Classes
        {
            Any = -1,
            [EnumMember(Value = "Unrated")] Unrated = 0,
            [EnumMember(Value = "Safe for work")] SafeForWork = 1,
            [EnumMember(Value = "Borderline safe for work")] Borderline = 2,
            [EnumMember(Value = "Not safe for work")] NotSafeForWork = 3
        }

        public enum Reported
        {
            Unreported = 0,
            Reported = 1,
            ConfirmedBad = 2,
            ConfirmedGood = 3,
            Lost = 4
        }

        public enum ResolutionSearch
        {
            Any = -1,
            Exact = 0,
            EqualOrGreaterThan = 1,
            SameAspectRatio = 2
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Sources
        {
            [EnumMember(Value = "4chan /w")] SlashW = 0,
            [EnumMember(Value = "4chan /v/")] SlashV = 1,
            [EnumMember(Value = "4chan /wg/")] SlashWG = 2,
            [EnumMember(Value = "4chan /hr/")] SlashHR = 3,

            [EnumMember(Value = "7chan /wp/")] SevenChan = 100,

            [EnumMember(Value = "reddit /r/wallpapers")] RedditWallpapers = 200,
            [EnumMember(Value = "reddit /r/wallpaper")] RedditWallpaper = 201,
            [EnumMember(Value = "reddit /r/NSFW_Wallpapers/")] RedditNsfwWallpapers = 202,
            [EnumMember(Value = "reddit /r/multiwall/")] RedditMultiwall = 203,
            [EnumMember(Value = "reddit /r/EarthPorn/")] RedditEarthPorn = 204,
            [EnumMember(Value = "reddit /r/nocontext_wallpapers/")] RedditNoContextWallPapers = 205,
            [EnumMember(Value = "reddit /r/spaceporn")] RedditSpacePorn = 206,
            [EnumMember(Value = "reddit /r/HI_Res")] RedditHiRes = 207,
            [EnumMember(Value = "reddit /r/WidescreenWallpaper/")] RedditWidescreenWallpaper = 208,
            [EnumMember(Value = "reddit /r/WQHD_Wallpaper/")] RedditWQHDWallpaper = 209,
            [EnumMember(Value = "reddit /r/4to3Wallpapers/")] Reddit4to3Wallpapers = 210,
            [EnumMember(Value = "reddit /r/wallpaperdump/")] RedditWallpaperDump = 211,

            [EnumMember(Value = "8chan /w/")] SlashW8Chan = 300,
            [EnumMember(Value = "8chan /wg/")] SlashWG8Chan = 301,


            [EnumMember(Value = "imgur")] Imgur = 9998,
            Any = 9999
        }
    }
}