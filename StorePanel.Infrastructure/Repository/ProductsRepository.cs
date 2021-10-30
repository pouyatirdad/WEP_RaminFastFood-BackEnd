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
    public interface IProductsRepository : IBaseRepository<Product>
    {
        List<Product> GetProducts();
        Product GetProduct(int id);
        List<ProductMainFeature> GetProductMainFeatures(int id);
        List<ProductFeatureValue> GetProductFeatures(int id);
        List<SubFeature> GetSubFeaturesByFeatureId(int id);
        ProductMainFeature AddProductMainFeature(ProductMainFeature mainFeature);
        ProductFeatureValue AddProductFeature(ProductFeatureValue feature);
        List<Product> GetNewestProducts(int? take=null, int? skip = null);
        List<Product> SearchProducts(string txt);
        List<Product> GetHighestRate(int? take);
    }

    public class ProductsRepository : BaseRepository<Product>, IProductsRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public ProductsRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Product> GetProducts()
        {
            return _context.Products.Where(p => p.IsDeleted == false).Include(a => a.ProductGroup).OrderByDescending(a => a.InsertDate).ToList();
        }

        public Product GetProduct(int id)
        {
            var product = _context.Products.Include(p => p.ProductMainFeatures).Include(p => p.ProductFeatureValues).Include(p => p.Brand).Include(a => a.ProductGroup)
                .FirstOrDefault(p => p.Id == id);
            product.ProductMainFeatures = product.ProductMainFeatures.Where(f => f.IsDeleted == false).ToList();
            product.ProductFeatureValues = product.ProductFeatureValues.Where(f => f.IsDeleted == false).ToList();
            return product;
        }

        public List<ProductMainFeature> GetProductMainFeatures(int id)
        {
            return _context.ProductMainFeatures.Where(p => p.ProductId == id && p.IsDeleted == false).ToList();
        }
        public List<ProductFeatureValue> GetProductFeatures(int id)
        {
            return _context.ProductFeatureValues.Where(p => p.ProductId == id && p.IsDeleted == false).ToList();
        }
        public List<SubFeature> GetSubFeaturesByFeatureId(int id)
        {
            return _context.SubFeatures.Where(p => p.IsDeleted == false && p.FeatureId == id).ToList();
        }

        public ProductMainFeature AddProductMainFeature(ProductMainFeature mainFeature)
        {
            mainFeature.InsertDate = DateTime.Now;
            mainFeature.InsertUser = GetCurrentUsersName();
            _context.ProductMainFeatures.Add(mainFeature);
            _context.SaveChanges();

            _logger.LogEvent(mainFeature.GetType().Name, mainFeature.Id, "Add");
            return mainFeature;
        }
        public ProductFeatureValue AddProductFeature(ProductFeatureValue feature)
        {
            feature.InsertDate = DateTime.Now;
            feature.InsertUser = GetCurrentUsersName();
            _context.ProductFeatureValues.Add(feature);
            _context.SaveChanges();

            _logger.LogEvent(feature.GetType().Name, feature.Id, "Add");
            return feature;
        }
        public List<Product> GetNewestProducts(int? take=null, int? skip = null)
        {
            List<Product> products;

            if (take==null)
            {
                products = _context.Products.Where(p => p.IsDeleted == false).OrderByDescending(p => p.InsertDate).ToList();
            }
            else
            {
                if (skip == null)
                    products = _context.Products.Where(p => p.IsDeleted == false).OrderByDescending(p => p.InsertDate)
                    .Take(take.Value).ToList();
                else
                    products = _context.Products.Where(p => p.IsDeleted == false).OrderByDescending(p => p.InsertDate).Skip(skip.Value)
                        .Take(take.Value).ToList();
            }
            
            return products;
        }
        public List<Product> SearchProducts(string txt)
        {
            var data = _context.Products.Where(x => x.Title.Contains(txt) || x.ShortDescription.Contains(txt) || x.Description.Contains(txt)).ToList();
            return data;
        }
        public  List<Product> GetHighestRate(int? take=null)
        {
            var data = _context.Products.Where(x => x.IsDeleted==false).OrderByDescending(x=>x.Rate).Take(take.Value).ToList();
            return data;
        }
    }
}
