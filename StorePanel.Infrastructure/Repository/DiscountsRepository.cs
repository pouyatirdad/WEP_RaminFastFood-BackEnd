using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using StorePanel.Infrastructure.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Infrastructure.Repository
{

    public interface IDiscountsRepository : IBaseRepository<Discount>
    {
        List<Discount> GetDistinctedDiscounts();
        List<Discount> GetDiscountGroup(int id);
        List<Discount> GetDiscountsByGroupIdentifier(string groupIdentifier);

        Discount GetProductDiscount(int productId);
        Discount GetProductGroupDiscount(int productGroupId);
        Discount GetBrandDiscount(int brandId);
    }

    public class DiscountsRepository : BaseRepository<Discount>, IDiscountsRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public DiscountsRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Discount> GetDistinctedDiscounts()
        {
            return _context.Discounts.Where(d => d.IsDeleted == false).DistinctBy(d => d.GroupIdentifier).ToList();
        }
        public List<Discount> GetDiscountGroup(int id)
        {
            var discount = _context.Discounts.Find(id);
            return _context.Discounts.Where(d => d.IsDeleted == false && d.GroupIdentifier == discount.GroupIdentifier).ToList();
        }
        public List<Discount> GetDiscountsByGroupIdentifier(string groupIdentifier)
        {
            return _context.Discounts.Where(d => d.IsDeleted == false && d.GroupIdentifier == groupIdentifier).ToList();
        }

        public Discount GetProductDiscount(int productId)
        {
            return _context.Discounts.FirstOrDefault(d => d.IsDeleted == false && d.ProductId == productId);
        }
        public Discount GetProductGroupDiscount(int productGroupId)
        {
            return _context.Discounts.FirstOrDefault(d => d.IsDeleted == false && d.ProductGroupId == productGroupId);
        }
        public Discount GetBrandDiscount(int brandId)
        {
            return _context.Discounts.FirstOrDefault(d => d.IsDeleted == false && d.BrandId == brandId);
        }

    }
}
