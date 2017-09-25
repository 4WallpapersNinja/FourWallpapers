using FourWallpapers.Core;

namespace FourWallpapers.Models.Requests
{
    public class TopRequest
    {
        public string By { get; set; }
        public Enums.Classes Class { get; set; }
        public Enums.Sources Source { get; set; }
        public int PerPage { get; set; } = 100;
    }
}