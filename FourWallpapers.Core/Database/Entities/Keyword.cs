using System.ComponentModel.DataAnnotations.Schema;

namespace FourWallpapers.Core.Database.Entities
{
    [Table("Keyword")]
    public class Keyword : BaseEntity
    {
        public decimal IdKey { get; set; }

        public string Value { get; set; }
    }
}