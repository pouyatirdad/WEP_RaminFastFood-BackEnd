using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StorePanel.Core.Models
{
    [Table("SystemParameters")]
    public class SystemParameter : BaseEntity
    {
        [Display(Name = "شاخص")]
        public string Key { get; set; }
        [Display(Name = "مقدار")]
        public string Value { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
