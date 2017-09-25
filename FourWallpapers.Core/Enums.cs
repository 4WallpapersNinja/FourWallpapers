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
            [EnumMember(Value = "/w/")] SlashW = 0,
            [EnumMember(Value = "/v/")] SlashV = 1,
            [EnumMember(Value = "/wg/")] SlashWG = 2,
            [EnumMember(Value = "/hr/")] SlashHR = 3,

            [EnumMember(Value = "/wp/")] SevenChan = 100,

            [EnumMember(Value = "/r/wallpapers")] RedditWallpapers = 200,
            [EnumMember(Value = "/r/wallpaper")] RedditWallpaper = 201,
            [EnumMember(Value = "/r/NSFW_Wallpapers/")] RedditNsfwWallpapers = 202,
            [EnumMember(Value = "/r/multiwall/")] RedditMultiwall = 203,
            [EnumMember(Value = "/r/EarthPorn/")] RedditEarthPorn = 204,
            [EnumMember(Value = "/r/nocontext_wallpapers/")] RedditNoContextWallPapers = 205,
            [EnumMember(Value = "/r/spaceporn")] RedditSpacePorn = 206,
            [EnumMember(Value = "/r/HI_Res")] RedditHiRes = 207,
            [EnumMember(Value = "/r/WidescreenWallpaper/")] RedditWidescreenWallpaper = 208,
            [EnumMember(Value = "/r/WQHD_Wallpaper/")] RedditWQHDWallpaper = 209,
            [EnumMember(Value = "/r/4to3Wallpapers/")] Reddit4to3Wallpapers = 210,
            [EnumMember(Value = "/r/wallpaperdump/")] RedditWallpaperDump = 211,

            [EnumMember(Value = "imgur")] Imgur = 9998,
            Any = 9999
        }
    }
}