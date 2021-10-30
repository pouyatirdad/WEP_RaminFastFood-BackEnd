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
    public class FaqCategoryController : Controller
    {
        private readonly IFaqCategoryRepository _faqCategoryRepo;
        public FaqCategoryController(IFaqCategoryRepository faqCategoryRepo)
        {
            _faqCategoryRepo = faqCategoryRepo;
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
            var parser = new Parser<FaqCategory>(Request.Form, _faqCategoryRepo.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public IActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Create(FaqCategory model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _faqCategoryRepo.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            return PartialView(await _faqCategoryRepo.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(FaqCategory model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _faqCategoryRepo.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _faqCategoryRepo.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _faqCategoryRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
