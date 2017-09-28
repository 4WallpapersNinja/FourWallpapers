using System;
using System.Collections.Generic;
using System.Text;

namespace FourWallpapers.Core.Helpers
{
    public static class Utilities
    {
        public static string ByteToString(byte[] hash)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++) sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }

        public static decimal RatioCalculate(int x, int y)
        {
            return y == 0 ? x : RatioCalculate(y, x % y);
        }
    }
}
