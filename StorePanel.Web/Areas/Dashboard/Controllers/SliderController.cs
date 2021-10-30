using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class SliderController : Controller
    {
        private readonly ISliderRepository _sliderRepository;

        public SliderController(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

        public IActionResult Index(bool root = false)
        {
            ViewBag.Root = root;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public string LoadGrid()
        {
            var parser = new Parser<Slider>(Request.Form, _sliderRepository.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }


        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile Logo, Slider model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _sliderRepository.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Brand = await _sliderRepository.GetById(id);

            return PartialView(Brand);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Slider model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _sliderRepository.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _sliderRepository.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sliderRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
