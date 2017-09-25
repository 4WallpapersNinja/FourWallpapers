using System.Linq;
using FourWallpapers.Core;

namespace FourWallpapers.Models.Requests
{
    public class SearchRequest
    {
        public Enums.Classes Class { get; set; }
        public string Resolution { get; set; }
        public Enums.ResolutionSearch ResolutionSearch { get; set; }
        public Enums.Sources Source { get; set; }
        public string Criteria { get; set; }
        public string Ratio { get; set; }
        public decimal Size { get; set; } = 0;

        public string[] Keywords
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Criteria)) return new string[] { };
                return Criteria.ToLower().Contains(",") ? Criteria.Split(',') : new[] {Criteria.ToLower()};
            }
        }

        public int X => int.TryParse(Resolution.Split('x').First(), out var xAxis) ? xAxis : 1280;
        public int Y => int.TryParse(Resolution.Split('x').Last(), out var yAxis) ? yAxis : 720;

        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 100;
    }
}