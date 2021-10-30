using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace StorePanel.Core.Models
{
    public class User : IdentityUser
    {
        public string Avatar { get; set; }
        [MaxLength(300)]
        public string FirstName { get; set; }
        [MaxLength(300)]
        public string LastName { get; set; }
        public string Information { get; set; }

        //public ICollection<Article> Articles { get; set; }
    }
}
