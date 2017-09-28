using System;
using System.Collections.Generic;
using System.Text;

namespace FourWallpapers.Core.Database.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string TwoFactorAuth { get; set; }

        public UserRole[] Roles { get; set; }
    }
}
