using System;

namespace FourWallpapers.Helpers {
    public class Time {
        /// <summary>
        ///     Converts a valid long unixTime into its .net equivalent Datetime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(long unixTime) {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }
}