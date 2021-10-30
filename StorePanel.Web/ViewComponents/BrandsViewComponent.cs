using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class BrandsViewComponent :ViewComponent
    {
        private readonly IBrandsRepository _brandsRepository;

        public BrandsViewComponent(IBrandsRepository brandsRepository)
        {
            _brandsRepository = brandsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _brandsRepository.GetAll());
        }
    }
}
