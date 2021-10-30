using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Helpers;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class BasicdefinitionsController : Controller
    {
        private readonly IBasicdefinitionsRepository _basicdefinitionsRepository;

        private UserManager<User> _userManager;

        public BasicdefinitionsController(IBasicdefinitionsRepository basicdefinitionsRepository
        ,UserManager<User> userManager)
        {
            _basicdefinitionsRepository = basicdefinitionsRepository;

            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model =await _basicdefinitionsRepository.GetById(1);
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Basicdefinitions model, IFormFile Logo)
        {
            if (!ModelState.IsValid)
                return View(model);


            if (Logo != null)
            {
                var imageName = await ImageHelper.SaveImage(Logo, 670, 400, true);
                model.Logo = imageName;
            }
            model.InsertDate = DateTime.Now;

            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser != null)
                model.InsertUser = currentUser.UserName;

            await _basicdefinitionsRepository.AddOrUpdate(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
