using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using StorePanel.Infrastructure.Service;
using StorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class NewestProductViewComponent :ViewComponent
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductService _productService;

        public NewestProductViewComponent(IProductsRepository productsRepository, IProductService productService)
        {
            _productsRepository = productsRepository;
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var products = _productService.GetLatestProductsWithPrice(8).Result;
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
                vm.Add(new ProductWithPriceViewModel(product));
            return View(vm);
        }
    }
}
