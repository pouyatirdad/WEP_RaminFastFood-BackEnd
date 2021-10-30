using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using StorePanel.Infrastructure.Context;
using StorePanel.Infrastructure.Dtos.Product;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Infrastructure.Service
{
    public interface IProductService
    {
        List<Product> GetTopSoldProducts(int take);

        int GetProductSoldCount(Product product);

        int GetProductStockCount(int productId);
        Task<int> GetProductStockCount(int productId, int mainFeatureId);
        long GetProductPrice(Product product);
        long GetProductPrice(Product product, int mainFeatureId);
        long GetProductPriceAfterDiscount(Product product);
        long GetProductPriceAfterDiscount(Product product, int mainFeatureId);
        Task<List<ProductWithPriceDto>> GetTopSoldProductsWithPrice(int take);
        Task<List<ProductWithPriceDto>> GetRelatedProducts(int productId, int take);
        Task<List<ProductWithPriceDto>> GetLatestProductsWithPrice(int? take = null, int? skip = null);
        Task<List<ProductWithPriceDto>> SearchProductWithPrice(string str);

        Task<ProductWithPriceDto> CreateProductWithPriceDto(int productId);
        Task<ProductWithPriceDto> CreateProductWithPriceDto(int productId, int mainFeatureId);
        Task<ProductWithPriceDto> CreateProductWithPriceDto(Product product);
        Task<ProductWithPriceDto> CreateProductWithPriceDto(Product product, int mainFeatureId);
        List<Product> GetHighRateProducts(int take);
        Task<List<ProductWithPriceDto>> GetHighRateProductsWithPrice(int take);

        List<int> GetAllChildrenProductGroupIds(int id);

        List<ProductGroupWithProductCountDto> GetPopularProductGroups(int take);

        List<Product> GetProductsGrid(int? productGroupId, List<int> brandIds = null, List<int> subFeatureIds = null, long? fromPrice = null, long? toPrice = null, string searchString = null);


    }

    public class ProductService : IProductService
    {
        private readonly IInvoicesRepository _InvoiceRepo;
        private readonly IProductsRepository _productRepo;
        private readonly IProductGroupsRepository _productGroupRepo;
        private readonly IProductMainFeaturesRepository _productMainFeatureRepo;
        private readonly IDiscountsRepository _discountRepo;
        private readonly StorePanelDbContext _context;
        public ProductService(IProductsRepository productRepo,
            IInvoicesRepository invoiceRepo,
            IProductMainFeaturesRepository productMainFeatureRepo,
            IDiscountsRepository discountRepo, StorePanelDbContext context,
            IProductGroupsRepository productGroupRepo)
        {
            _productRepo = productRepo;
            _InvoiceRepo = invoiceRepo;
            _productMainFeatureRepo = productMainFeatureRepo;
            _discountRepo = discountRepo;
            _context = context;
            _productGroupRepo = productGroupRepo;
        }
        public List<Product> GetTopSoldProducts(int take)
        {
            var products = new List<Product>();
            var TopSoldProducts = _InvoiceRepo.GertTopSoldProducts(take);
            if (TopSoldProducts.Any() == false)
                products = _productRepo.GetNewestProducts(take);
            else
            {
                products = TopSoldProducts;
            }

            return products;
        }
        public List<Product> GetHighRateProducts(int take)
        {
            //var products = new List<Product>();
            var products = _productRepo.GetHighestRate(take);
            //if (TopSoldProducts.Any() == false)
            //    products = _productRepo.GetNewestProducts(take);
            //else
            //{
            //    products = TopSoldProducts;
            //}

            return products;
        }

        public int GetProductSoldCount(Product product)
        {
            var amount = 0;
            var invoices = _context.InvoiceItems.Where(i => i.ProductId == product.Id && i.IsDeleted == false).ToList();
            if (invoices.Any())
                amount += invoices.Sum(i => i.Quantity);
            return amount;
        }

        public int GetProductStockCount(int productId)
        {
            var inStock = 0;
            var mainFeature = _productMainFeatureRepo.GetByProductId(productId);
            if (mainFeature != null)
                inStock += mainFeature.Quantity;
            return inStock;
        }
        public async Task<int> GetProductStockCount(int productId, int mainFeatureId)
        {
            var inStock = 0;
            var mainFeature = await _productMainFeatureRepo.GetById(mainFeatureId);
            if (mainFeature != null)
                inStock += mainFeature.Quantity;
            return inStock;
        }
        public long GetProductPrice(Product product)
        {
            long price = 0;
            var mainFeature = _productMainFeatureRepo.GetByProductId(product.Id);
            if (mainFeature != null && mainFeature.Quantity > 0)
            {
                price = mainFeature.Price;
            }

            return price;
        }
        public int GetProductMainFeaturesID(Product product)
        {
            int mainfeatureId = 0;
            var mainFeature = _productMainFeatureRepo.GetProductMainFeaturesID(product.Id);
            if (mainFeature != null )
            {
                mainfeatureId = mainFeature.Id;
            }

            return mainfeatureId;
        }
        public long GetProductPrice(Product product, int mainFeatureId)
        {
            long price = 0;
            var mainFeature = _productMainFeatureRepo.GetByProductId(product.Id, mainFeatureId);
            if (mainFeature != null && mainFeature.Quantity > 0)
            {
                price = mainFeature.Price;
            }

            return price;

        }
        public long GetProductPriceAfterDiscount(Product product)
        {
            var productPrice = GetProductPrice(product);
            var priceAfterDiscount = productPrice;

            // Checking For Product Discount
            var discount = _discountRepo.GetProductDiscount(product.Id);

            // Checking For ProductGroupDiscount
            if (discount == null)
                discount = _discountRepo.GetProductGroupDiscount(product.ProductGroupId ?? 0);

            // Checking For Brand Discount
            if (discount == null)
                discount = _discountRepo.GetBrandDiscount(product.BrandId ?? 0);

            if (discount != null)
            {
                if (discount.DiscountType == (int)DiscountType.Amount)
                {
                    priceAfterDiscount -= discount.Amount;
                }
                else if (discount.DiscountType == (int)DiscountType.Percentage)
                {
                    var discountAmount = (discount.Amount * productPrice / 100);
                    priceAfterDiscount -= discountAmount;
                }
            }

            return priceAfterDiscount;
        }
        public long GetProductPriceAfterDiscount(Product product, int mainFeatureId)
        {
            var productPrice = GetProductPrice(product, mainFeatureId);
            var priceAfterDiscount = productPrice;

            // Checking For Product Discount
            var discount = _discountRepo.GetProductDiscount(product.Id);

            // Checking For ProductGroupDiscount
            if (discount == null)
                discount = _discountRepo.GetProductGroupDiscount(product.ProductGroupId ?? 0);

            // Checking For Brand Discount
            if (discount == null)
                discount = _discountRepo.GetBrandDiscount(product.BrandId ?? 0);

            if (discount != null)
            {
                if (discount.DiscountType == (int)DiscountType.Amount)
                {
                    priceAfterDiscount -= discount.Amount;
                }
                else if (discount.DiscountType == (int)DiscountType.Percentage)
                {
                    var discountAmount = (discount.Amount * productPrice / 100);
                    priceAfterDiscount -= discountAmount;
                }
            }

            return priceAfterDiscount;
        }
        public async Task<List<ProductWithPriceDto>> GetTopSoldProductsWithPrice(int take)
        {
            var productsDto = new List<ProductWithPriceDto>();
            var products = GetTopSoldProducts(take);

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var productDto = await CreateProductWithPriceDto(product);
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }
        public async Task<List<ProductWithPriceDto>> GetHighRateProductsWithPrice(int take)
        {
            var productsDto = new List<ProductWithPriceDto>();
            var products = GetHighRateProducts(take);

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var productDto = await CreateProductWithPriceDto(product);
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }
        public async Task<List<ProductWithPriceDto>> GetRelatedProducts(int productId, int take)
        {
            var productt = await _productRepo.GetById(productId);
            var relatedProducts = _context.Products
                .Where(p => p.ProductGroupId == productt.ProductGroupId && p.IsDeleted == false && p.Id != productId)
                .Take(take).ToList();
            var productsDto = new List<ProductWithPriceDto>();

            #region Getting Product Price And Discount

            foreach (var product in relatedProducts)
            {
                var productDto = await CreateProductWithPriceDto(product);
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }
        public async Task<List<ProductWithPriceDto>> GetLatestProductsWithPrice(int? take=null, int? skip = null)
        {

            var productsDto = new List<ProductWithPriceDto>();
            var products = new List<Product> {};
            if (take==null)
            {
                 products = _productRepo.GetNewestProducts();
            }
            else
            {
                 products = _productRepo.GetNewestProducts(take.Value, skip);
            }

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var productDto = await CreateProductWithPriceDto(product);
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }
        public async Task<List<ProductWithPriceDto>> SearchProductWithPrice(string str)
        {

            var productsDto = new List<ProductWithPriceDto>();
            var products = _productRepo.SearchProducts(str);

            #region Getting Product Price And Discount

            foreach (var product in products)
            {
                var productDto = await CreateProductWithPriceDto(product);
                productsDto.Add(productDto);
            }
            #endregion
            return productsDto;
        }
        public async Task<ProductWithPriceDto> CreateProductWithPriceDto(int productId)
        {
            var product = await _productRepo.GetById(productId);
            var productGroup = await _productGroupRepo.GetById(product.ProductGroupId.Value);
            var price = GetProductPrice(product);
            var priceAfterDiscount = GetProductPriceAfterDiscount(product);
            int discountPercentage = 0;
            if (price != 0 && priceAfterDiscount != 0)
                discountPercentage = (int)(100 - (priceAfterDiscount * 100 / price));

            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.ShortDescription,
                ProductGroupId = productGroup.Id,
                ProductGroupName = productGroup.Title,
                Rate = product.Rate,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = priceAfterDiscount,
                DiscountPercentage = discountPercentage
            };
            return productDto;
        }
        public async Task<ProductWithPriceDto> CreateProductWithPriceDto(int productId, int mainFeatureId)
        {
            var product = await _productRepo.GetById(productId);
            var productGroup = await _productGroupRepo.GetById(product.ProductGroupId.Value);
            var price = GetProductPrice(product, mainFeatureId);
            var priceAfterDiscount = GetProductPriceAfterDiscount(product, mainFeatureId);
            int discountPercentage = 0;
            if (price != 0 && priceAfterDiscount != 0)
                discountPercentage = (int)(100 - (priceAfterDiscount * 100 / price));

            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.ShortDescription,
                ProductGroupId = productGroup.Id,
                ProductGroupName = productGroup.Title,
                Rate = product.Rate,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = priceAfterDiscount,
                DiscountPercentage = discountPercentage
            };
            return productDto;
        }
        public async Task<ProductWithPriceDto> CreateProductWithPriceDto(Product product)
        {
            var productGroup = await _productGroupRepo.GetById(product.ProductGroupId.Value);
            var price = GetProductPrice(product);
            var mainFeature = GetProductMainFeaturesID(product);
            var priceAfterDiscount = GetProductPriceAfterDiscount(product);
            int discountPercentage = 0;
            if (price != 0 && priceAfterDiscount != 0)
                discountPercentage = (int)(100 - (priceAfterDiscount * 100 / price));

            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.Title,
                ProductGroupId = productGroup.Id,
                ProductGroupName = productGroup.Title,
                MainFeatureId = mainFeature,
                Rate = product.Rate,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = priceAfterDiscount,
                DiscountPercentage = discountPercentage
            };
            return productDto;
        }
        public async Task<ProductWithPriceDto> CreateProductWithPriceDto(Product product, int mainFeatureId)
        {
            var productGroup = await _productGroupRepo.GetById(product.ProductGroupId.Value);
            var price = GetProductPrice(product, mainFeatureId);
            var priceAfterDiscount = GetProductPriceAfterDiscount(product, mainFeatureId);
            int discountPercentage = 0;
            if (price != 0 && priceAfterDiscount != 0)
                discountPercentage = (int)(100 - (priceAfterDiscount * 100 / price));

            var productDto = new ProductWithPriceDto()
            {
                Id = product.Id,
                ShortTitle = product.ShortDescription,
                ProductGroupId = productGroup.Id,
                ProductGroupName = productGroup.Title,
                Rate = product.Rate,
                Image = product.Image,
                Price = price,
                PriceAfterDiscount = priceAfterDiscount,
                DiscountPercentage = discountPercentage
            };
            return productDto;
        }
        public List<int> GetAllChildrenProductGroupIds(int id)
        {
            var ids = new List<int>();
            ids.AddRange(_context.ProductGroups.Where(p => p.IsDeleted == false && p.ParentId == id).Select(p => p.Id).ToList());
            foreach (var item in ids.ToList())
            {
                var childIds = GetAllChildrenProductGroupIds(item);
                if (childIds.Any())
                {
                    ids.AddRange(childIds);
                }
            }
            return ids;
        }

        public List<ProductGroupWithProductCountDto> GetPopularProductGroups(int take)
        {
            var dto = new List<ProductGroupWithProductCountDto>();
            var productGroups = _context.ProductGroups.Where(p => p.ParentId != null && p.IsDeleted == false).Take(take)
                .ToList();
            foreach (var item in productGroups)
            {
                var count = _context.Products.Count(p => p.IsDeleted == false && p.ProductGroupId == item.Id);
                var childIds = GetAllChildrenProductGroupIds(item.Id);
                count += childIds.Sum(id => _context.Products.Count(p => p.IsDeleted == false && p.ProductGroupId == id));
                dto.Add(new ProductGroupWithProductCountDto()
                {
                    ProductGroup = item,
                    ProductCount = count
                });
            }

            return dto;
        }
        #region Get Products Grid

        public List<Product> GetProductsGrid(int? productGroupId, List<int> brandIds = null, List<int> subFeatureIds = null, long? fromPrice = null, long? toPrice = null, string searchString = null)
        {
            var products = new List<Product>();
            var count = 0;
            if (productGroupId == null || productGroupId == 0)
            {
                if (string.IsNullOrEmpty(searchString))
                {
                    products = _context.Products.Include(p => p.ProductMainFeatures).Include(p => p.ProductFeatureValues).Where(p => p.IsDeleted == false).OrderByDescending(p => p.InsertDate).ToList();
                }
                else
                {
                    products = _context.Products.Include(p => p.ProductMainFeatures)
                        .Include(p => p.ProductFeatureValues)
                        .Where(p => p.IsDeleted == false && (p.ShortDescription.Trim().ToLower().Contains(searchString.Trim().ToLower()) || p.Title.Trim().ToLower().Contains(searchString.Trim().ToLower())))
                        .OrderByDescending(p => p.InsertDate).ToList();
                }
            }
            else
            {
                products = _context.Products.Include(p => p.ProductMainFeatures).Include(p => p.ProductFeatureValues).Where(p => p.IsDeleted == false && p.ProductGroupId == productGroupId).OrderByDescending(p => p.InsertDate).ToList();

                var allChildrenGroups = GetAllChildrenProductGroupIds(productGroupId.Value);
                foreach (var groupId in allChildrenGroups)
                    products.AddRange(_context.Products.Where(p => p.IsDeleted == false && p.ProductGroupId == groupId).OrderByDescending(p => p.InsertDate).ToList());
                if (string.IsNullOrEmpty(searchString) == false)
                {
                    products = products
                        .Where(p => p.IsDeleted == false && (p.ShortDescription.Trim().ToLower().Contains(searchString.Trim().ToLower()) || p.Title.Trim().ToLower().Contains(searchString.Trim().ToLower())))
                        .OrderByDescending(p => p.InsertDate).ToList();
                }
            }

            if (brandIds != null && brandIds.Any())
            {
                var productsFilteredByBrand = new List<Product>();
                foreach (var brand in brandIds)
                    productsFilteredByBrand.AddRange(products.Where(p => p.IsDeleted == false && p.BrandId == brand).OrderByDescending(p => p.InsertDate).ToList());
                products = productsFilteredByBrand;
            }
            if (subFeatureIds != null && subFeatureIds.Any(f => f != 0))
            {
                var productsFilteredByFeature = new List<Product>();
                foreach (var subFeature in subFeatureIds.Where(f => f != 0))
                    productsFilteredByFeature.AddRange(products.Where(p => p.ProductFeatureValues.Any(pf => pf.SubFeatureId == subFeature) || p.ProductMainFeatures.Any(pf => pf.SubFeatureId == subFeature)).OrderByDescending(p => p.InsertDate).ToList());
                products = productsFilteredByFeature;
            }

            if (fromPrice != null)
                products = products.Where(p => GetProductPriceAfterDiscount(p) >= fromPrice).ToList();

            if (toPrice != null)
                products = products.Where(p => GetProductPriceAfterDiscount(p) <= toPrice).ToList();

            return products;
        }
        #endregion
    }
}
