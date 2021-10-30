using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface IProductMainFeaturesRepository : IBaseRepository<ProductMainFeature>
    {
        ProductMainFeature GetByProductId(int productId);
        ProductMainFeature GetByProductId(int productId, int mainFeatureId);
        ProductMainFeature GetProductMainFeature(int productId);
        ProductMainFeature GetProductMainFeaturesID(int productId);
        void UpdateQuantity(int id, int newquantity);
        List<ProductMainFeature> productMainFeatures(int SubFeatuerId, int ProductGroupId);
        public List<ProductMainFeature> GetProductMainFeatures(int productId);
    }

    public class ProductMainFeaturesRepository : BaseRepository<ProductMainFeature>,IProductMainFeaturesRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public ProductMainFeaturesRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public ProductMainFeature GetByProductId(int productId)
        {
            return _context.ProductMainFeatures.FirstOrDefault(f => f.IsDeleted == false && f.ProductId == productId);
        }
        public ProductMainFeature GetProductMainFeaturesID(int productId)
        {
            return _context.ProductMainFeatures.FirstOrDefault(x => x.IsDeleted == false && x.ProductId == productId);
        }
        public ProductMainFeature GetByProductId(int productId, int mainFeatureId)
        {
            return _context.ProductMainFeatures.FirstOrDefault(f => f.IsDeleted == false && f.ProductId == productId && f.Id == mainFeatureId);
        }
        public ProductMainFeature GetProductMainFeature(int productId)
        {
            return _context.ProductMainFeatures.Include(f => f.Feature).Include(f => f.SubFeature).Where(f => f.IsDeleted == false && f.ProductId == productId).FirstOrDefault();
        }

        public List<ProductMainFeature> GetProductMainFeatures(int productId)
        {
            return _context.ProductMainFeatures.Include(f => f.Feature).Include(f => f.SubFeature).Where(f => f.IsDeleted == false && f.ProductId == productId).ToList();
        }

        public void UpdateQuantity(int id, int newquantity)
        {
            var oldquantity = _context.ProductMainFeatures.Where(a => a.Id == id).FirstOrDefault();
            oldquantity.Quantity = oldquantity.Quantity - newquantity;
            _context.Entry(oldquantity).State = EntityState.Modified;
            _context.SaveChanges();
            _logger.LogEvent(oldquantity.GetType().Name, oldquantity.Id, "Update");


        }
        public List<ProductMainFeature> productMainFeatures(int SubFeatuerId, int ProductGroupId)
        {
            var productsubfeature = _context.ProductMainFeatures.Where(a => a.SubFeatureId == SubFeatuerId).Include(a => a.Product).ToList();
            var qq = productsubfeature.Where(a => a.Product.ProductGroupId == ProductGroupId).ToList();
            return qq;
        }
    }
}
