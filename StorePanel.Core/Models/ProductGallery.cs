using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Core.Models
{
    public class ProductGallery : BaseEntity
    { 
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        [Display(Name = "عنوان تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
