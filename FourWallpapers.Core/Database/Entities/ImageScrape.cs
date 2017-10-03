using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourWallpapers.Core.Database.Entities
{
    [Table("ImageScrape")]
    public class ImageScrape : BaseEntity
    {
        public string ImageId { get; set; }
        public Enums.Sources Source { get; set; }
        public string Hash { get; set; }
        public Guid ScrapeId { get; set; }
        public DateTime Datestamp { get; set; }
    }
}