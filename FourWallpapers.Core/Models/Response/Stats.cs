using System;
using System.Collections.Generic;

namespace FourWallpapers.Core.Models.Response
{
    public class StatsResponse
    {
        public List<Result> TopStats { get; set; } = new List<Result>();

        public List<Result> TopKeywords { get; set; } = new List<Result>();

        public DateTime AsOf { get; set; }
    }

    public class Result
    {
        public string Keyword { get; set; }
        public long Count { get; set; }
    }
}