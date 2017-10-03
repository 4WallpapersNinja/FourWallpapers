using System.Runtime.Serialization;
using FourWallpapers.Core.Database.Entities;

namespace FourWallpapers.Core.Models.Response
{
    [DataContract]
    public class SearchResult
    {
        public SearchResult(Image image)
        {
            ImageId = image.ImageId;
            FilePath = $"{ImageId.Substring(0,2)}/{ImageId}.{image.FileExtension}";
            Server = image.Server;
            IsThumbnailAvailable = image.IsThumbnailAvailable;
        }

        [DataMember]
        public string ImageId { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public bool IsThumbnailAvailable { get; set; }
    }
}