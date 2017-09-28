using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace FourWallpapers.Core.Database.Entities
{
    [Table("Image")]
    public class Image : BaseEntity
    {
        [DataMember]
        public string ImageId { get; set; }

        public decimal IdKey { get; set; }
        
        [DataMember]
        public string FileExtension { get; set; }

        public Enums.Classes Class { get; set; }

        [DataMember]
        [NotMapped]
        public Enums.Classes Classification => Class;

        [DataMember]
        public Enums.Sources IndexSource { get; set; }

        [DataMember]
        public string Who { get; set; }

        [NotMapped]
        public string Tripcode { get; set; }

        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }

        [DataMember]
        public string Resolution => $"{ResolutionX}x{ResolutionY}";

        [DataMember]
        [NotMapped]
        public string Tags => string.Join(",", Keywords ?? new List<string>());

        [NotMapped]
        public List<string> Keywords { get; set; } = new List<string>();

        public string TagsString { get; set; }

        [DataMember]
        public DateTime DateDownloaded { get; set; }

        [DataMember]
        public Enums.Reported Reported { get; set; }

        [DataMember]
        public string Ratio { get; set; }

        [DataMember]
        public int Downloads { get; set; } = 0;

        public string Hash { get; set; }

        public int ServerId { get; set; }

        public bool IsThumbnailAvailable { get; set; }
        public bool IsLockedImage { get; set; }
        public decimal Size { get; set; }

        [DataMember]
        [NotMapped]
        public string Server => ServerId.ToString().PadLeft(3, '0');
    }
}