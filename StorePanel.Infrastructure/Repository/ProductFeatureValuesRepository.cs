using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;


namespace StorePanel.Infrastructure.Repository
{

    public interface IProductFeatureValuesRepository : IBaseRepository<ProductFeatureValue>
    {
        List<ProductFeatureValue> GetProductFeatures(int productId);
    }
    public class ProductFeatureValuesRepository : BaseRepository<ProductFeatureValue>, IProductFeatureValuesRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public ProductFeatureValuesRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<ProductFeatureValue> GetProductFeatures(int productId)
        {
            return _context.ProductFeatureValues.Include(f => f.Feature).Include(f => f.SubFeature)
                .Where(f => f.IsDeleted == false && f.ProductId == productId).ToList();
        }
    }
}
