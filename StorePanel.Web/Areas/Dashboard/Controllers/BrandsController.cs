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
    public class BrandsController : Controller
    {
        private readonly IBrandsRepository _brandsRepository;

        public BrandsController(IBrandsRepository brandsRepository)
        {
            _brandsRepository = brandsRepository;
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
            var parser = new Parser<Brand>(Request.Form, _brandsRepository.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }


        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile Logo,Brand model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _brandsRepository.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var Brand = await _brandsRepository.GetById(id);

            return PartialView(Brand);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Brand model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _brandsRepository.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _brandsRepository.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _brandsRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
