using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Controllers
{
    public class PwaStorePanelController : Controller
    {
        [Route("StorePanelApp")]
        [Route("PwaStorePanel/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
