using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class ShopNameViewComponent : ViewComponent
    {
        private readonly IBasicdefinitionsRepository _basicdefinitionsRepository;

        public ShopNameViewComponent(IBasicdefinitionsRepository basicdefinitionsRepository)
        {
            _basicdefinitionsRepository = basicdefinitionsRepository;
        }

        public IViewComponentResult Invoke()
        {
            var name = _basicdefinitionsRepository.GetById(1).Result;
            return View(name);
        }
    }
}
