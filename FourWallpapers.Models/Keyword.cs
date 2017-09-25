using System.ComponentModel.DataAnnotations.Schema;

namespace FourWallpapers.Models
{
    [Table("Keyword")]
    public class Keyword : BaseEntity
    {
        public decimal IdKey { get; set; }

        public string Value { get; set; }
    }
}