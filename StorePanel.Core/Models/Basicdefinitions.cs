using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class Basicdefinitions : BaseEntity
    {
        [Display(Name = "ZarinPal Api Key")]
        public string ZarinPalApi { get; set; }
        [Display(Name = "لگو")]
        public string Logo { get; set; }
        [Display(Name = "نام فروشگاه")]
        public string NameShop { get; set; }

    }
}
