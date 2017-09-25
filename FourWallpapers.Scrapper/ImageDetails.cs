using FourWallpapers.Core;

namespace FourWallpapers.Scrapper
{
    public class ImageDetail
    {
        public string ImageUrl { get; set; }
        public Enums.Classes Class { get; set; } = Enums.Classes.Unrated;
        public string Who { get; set; }
        public Enums.Sources IndexSource { get; set; }
        public string Tag { get; set; }
        public string TripCode { get; set; }
        public string Resolution { get; set; }
        public string Hash { get; set; }
        public bool AlreadyExists { get; set; }
        public string ImageId { get; set; }
        public string ImageExtension { get; set; }
    }
}