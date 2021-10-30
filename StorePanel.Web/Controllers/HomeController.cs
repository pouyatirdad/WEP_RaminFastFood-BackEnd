using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStaticContentRepository _staticContentRepository;



        public HomeController(IStaticContentRepository staticContentRepository)
        {
            _staticContentRepository = staticContentRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Aboutus")]
        public async Task<IActionResult> Aboutus()
        {
            var model = _staticContentRepository.GetAll().Result[0];
            return View(model);
        }
    }
}
