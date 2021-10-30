using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{

    public interface IBrandsRepository : IBaseRepository<Brand>
    {
        List<Brand> brands(int? ProductGroupId);
        List<Brand> TakeSome(int? take);

    }

    public class BrandsRepository : BaseRepository<Brand>, IBrandsRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public BrandsRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<Brand> brands(int? ProductGroupId)
        {

            var brandid = _context.ProductGroupBrands.Where(a => a.IsDeleted == false).ToList();

            if (ProductGroupId != null)
            {
                brandid = _context.ProductGroupBrands.Where(a => a.ProductGroupId == ProductGroupId & a.IsDeleted == false).ToList();
            }

            var brandsname = new List<Brand>();
            foreach (var item in brandid)
            {
                var bb = _context.Brands.Where(a => a.Id == item.BrandId).FirstOrDefault();
                brandsname.Add(bb);
            }
            return brandsname;
        }
        public List<Brand> TakeSome(int? take)
        {
            return _context.Brands.Where(x => x.IsDeleted == false).Take(take.Value).ToList();
        }

    }
}
