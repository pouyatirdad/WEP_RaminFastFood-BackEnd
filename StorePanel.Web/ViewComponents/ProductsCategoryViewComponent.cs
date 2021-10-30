using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class ProductsCategoryViewComponent : ViewComponent
    {
        private readonly IProductGroupsRepository _productGroupsRepository;

        public ProductsCategoryViewComponent(IProductGroupsRepository productGroupsRepository)
        {
            _productGroupsRepository = productGroupsRepository;
        }

        public IViewComponentResult Invoke()
        {
            var model = _productGroupsRepository.GetProductGroupTable();
            return View(model);
        }
    }
}
