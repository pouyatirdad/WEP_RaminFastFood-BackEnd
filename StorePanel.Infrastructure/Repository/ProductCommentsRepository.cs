using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace StorePanel.Infrastructure.Repository
{
    public interface IProductCommentsRepository : IBaseRepository<ProductComment>
    {
        List<ProductComment> GetProductComments(int productId);
        string GetProductName(int productId);
        ProductComment DeleteComment(int id);
    }

    public class ProductCommentsRepository : BaseRepository<ProductComment>,IProductCommentsRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public ProductCommentsRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<ProductComment> GetProductComments(int productId)
        {
            return _context.ProductComments.Where(h => h.ProductId == productId & h.IsDeleted == false).ToList();
        }
        public string GetProductName(int productId)
        {
            return _context.Products.Find(productId).Title;
        }
        public ProductComment DeleteComment(int id)
        {
            var comment = _context.ProductComments.Find(id);
            var children = _context.ProductComments.Where(c => c.ParentId == id).ToList();
            foreach (var child in children)
            {
                child.IsDeleted = true;
                _context.Entry(child).State = EntityState.Modified;
                _context.SaveChanges();
            }
            comment.IsDeleted = true;
            _context.Entry(comment).State = EntityState.Modified;
            _context.SaveChanges();
            _logger.LogEvent(comment.GetType().Name, comment.Id, "Delete");
            return comment;
        }
    }
}