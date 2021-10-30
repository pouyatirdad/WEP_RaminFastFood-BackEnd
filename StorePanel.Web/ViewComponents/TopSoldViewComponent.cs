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
    public class TopSoldViewComponent : ViewComponent 
    {
        private readonly IInvoicesRepository _invoicesRepository;
        private readonly IProductService _productService;

        public TopSoldViewComponent(IInvoicesRepository invoicesRepository, IProductService productService)
        {
            _invoicesRepository = invoicesRepository;
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var products = _productService.GetTopSoldProductsWithPrice(8).Result;
            var vm = new List<ProductWithPriceViewModel>();
            foreach (var product in products)
                vm.Add(new ProductWithPriceViewModel(product));
            return View(vm);
        }
    }
}
