using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;

namespace StorePanel.Web.Areas.Customer.Components
{
    public class siteNameCU : ViewComponent
    {
        private readonly IBasicdefinitionsRepository _basicdefinitionsRepository;

        public siteNameCU(IBasicdefinitionsRepository basicdefinitionsRepository)
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
