using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class ContactUsFormController : Controller
    {
        private readonly IContactUsFormRepository _contactUsFormRepository;

        public ContactUsFormController(IContactUsFormRepository contactUsFormRepository)
        {
            _contactUsFormRepository = contactUsFormRepository;
        }

        public async Task<IActionResult> Index(bool root = false)
        {
            ViewBag.Root = root;
            var data = await _contactUsFormRepository.GetAll();
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public string LoadGrid()
        {
            var parser = new Parser<ContactForm>(Request.Form, _contactUsFormRepository.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }
    }
}
