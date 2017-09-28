namespace FourWallpapers.Core.Models.Request
{
    public class Top
    {
        public string By { get; set; }
        public Enums.Classes Class { get; set; }
        public Enums.Sources Source { get; set; }
        public int PerPage { get; set; } = 100;
    }
}