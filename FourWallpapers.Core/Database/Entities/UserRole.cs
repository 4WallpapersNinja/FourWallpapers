using System;
using System.Collections.Generic;
using System.Text;

namespace FourWallpapers.Core.Database.Entities
{
    public class UserRole : BaseEntity
    {
        public string Name { get; set; }
        
        public UserClaim[] Claims { get; set; }
    }
}
