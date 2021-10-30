using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class ContactForm : BaseEntity
    {
        [MaxLength(600)]
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد کنید")]
        public string Name { get; set; }
        [MaxLength(600)]
        [Display(Name = "موبایل")]
        [Required(ErrorMessage = "لطفا شماره {0} خود را وارد کنید")]
        public string Phone { get; set; }
        [MaxLength(600)]
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} خود را وارد کنید")]
        [EmailAddress(ErrorMessage = "{0} وارد شده معتبر نیست")]
        public string Email { get; set; }
        [Display(Name = "پیام")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
