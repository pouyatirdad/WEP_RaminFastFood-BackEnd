

namespace StorePanel.Web.Areas.Dashboard.ViewModels
{
    public class ArticleGridViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string PersianDate { get; set; }
    }
    public class CommentGridViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool IsShow { get; set; }
    }
}
