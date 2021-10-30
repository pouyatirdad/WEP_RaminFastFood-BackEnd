using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Core.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ViewCount { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }
    }
}
