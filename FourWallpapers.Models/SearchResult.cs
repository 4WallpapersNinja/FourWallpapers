using System.Runtime.Serialization;

namespace FourWallpapers.Models
{
    [DataContract]
    public class SearchResult : BaseEntity
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