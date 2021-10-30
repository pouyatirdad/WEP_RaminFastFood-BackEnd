using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class ShopinIndexViewComponent : ViewComponent 
    {
        private readonly IProductsRepository _iProductsRepository;
        private readonly IProductGroupsRepository _productGroupsRepository;

        public ShopinIndexViewComponent(IProductsRepository iProductsRepository, IProductGroupsRepository productGroupsRepository)
        {
            _iProductsRepository = iProductsRepository;
            _productGroupsRepository = productGroupsRepository;
        }

        public IViewComponentResult Invoke()
        {
            return View(_iProductsRepository.GetDefaultQuery().Where(x=>x.IsDeleted==false).Take(9).ToList());
        }
    }
}
