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
            FileExtension = image.FileExtension;
            Server = image.Server;
            IsThumbnailAvailable = image.IsThumbnailAvailable;
        }

        [DataMember]
        public string ImageId { get; set; }

        [DataMember]
        public string FileExtension { get; set; }

        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public bool IsThumbnailAvailable { get; set; }
    }
}