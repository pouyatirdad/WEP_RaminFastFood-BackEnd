using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class Slider : BaseEntity
    {
        [Display(Name = "نام")]
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
