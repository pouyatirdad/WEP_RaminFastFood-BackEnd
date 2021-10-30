using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class BrandsAboutusViewComponent : ViewComponent
    {
        private readonly IBrandsRepository _brandsRepository;

        public BrandsAboutusViewComponent(IBrandsRepository brandsRepository)
        {
            _brandsRepository = brandsRepository;
        }

        public IViewComponentResult Invoke()
        {
            return View(_brandsRepository.TakeSome(8));
        }
    }
}
