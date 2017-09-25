using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourWallpapers.Models
{
    [Table("User")]
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}