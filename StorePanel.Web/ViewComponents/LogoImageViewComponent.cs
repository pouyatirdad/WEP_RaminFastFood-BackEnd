using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class LogoImageViewComponent : ViewComponent
    {
        private readonly IBasicdefinitionsRepository _basicdefinitionsRepository;

        public LogoImageViewComponent(IBasicdefinitionsRepository basicdefinitionsRepository)
        {
            _basicdefinitionsRepository = basicdefinitionsRepository;
        }

        public IViewComponentResult Invoke()
        {
            var image = _basicdefinitionsRepository.GetById(1).Result;
            return View(image);
        }
    }
}
