using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Infrastructure.Repository
{
    public interface IProductGalleriesRepository : IBaseRepository<ProductGallery>
    {
        List<ProductGallery> GetProductGalleries(int productId);
        string GetProductName(int productId);
    }

    public class ProductGalleriesRepository : BaseRepository<ProductGallery>, IProductGalleriesRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly LogRepository _logger;
        public ProductGalleriesRepository(StorePanelDbContext context, LogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<ProductGallery> GetProductGalleries(int productId)
        {
            return _context.ProductGalleries.Where(h => h.ProductId == productId & h.IsDeleted == false).ToList();
        }
        public string GetProductName(int productId)
        {
            return _context.Products.Find(productId).Title;
        }
    }
}
