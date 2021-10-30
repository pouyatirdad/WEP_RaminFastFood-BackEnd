using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class StaticContentController : Controller
    {
        private readonly IStaticContentRepository _staticContentRepo;

        public StaticContentController(IStaticContentRepository staticContentRepo)
        {
            _staticContentRepo = staticContentRepo;
        }

        public IActionResult Index()
        {
            var staticContent = _staticContentRepo.GetDefaultQuery().FirstOrDefault() ?? new StaticContent();
            ViewBag.Saved = false;
            return View(staticContent);
        }
        [HttpPost]
        public async Task<IActionResult> Index(StaticContent model)
        {

                if (!ModelState.IsValid)
                    return View();

                await _staticContentRepo.AddOrUpdate(model);
            ViewBag.Saved = true;
                return View(); 
            
        }
    }
}
