using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorePanel.Core.Models
{
    public class Article : BaseEntity
    {
        [Display(Name = "عنوان مطلب")]
        [MaxLength(600, ErrorMessage = "{0} باید از 600 کارکتر کمتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "توضیح کوتاه")]
        [DataType(DataType.MultilineText)]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیح")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int ViewCount { get; set; }
        [Display(Name = "تصویر")]
        public string Image { get; set; }
        public DateTime? AddedDate { get; set; }

        public int? ArticleCategoryId { get; set; }
        public ArticleCategory ArticleCategory { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; }
        public ICollection<ArticleComment> ArticleComments { get; set; }

    }

}
