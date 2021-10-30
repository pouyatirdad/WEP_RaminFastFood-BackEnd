using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class FaqController : Controller
    {
        private readonly IFaqRepository _faqRepo;
        private readonly IFaqCategoryRepository _faqCategoryRepo;
        public FaqController(IFaqRepository faqRepo, IFaqCategoryRepository faqCategoryRepo)
        {
            _faqRepo = faqRepo;
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
            var parser = new Parser<Faq>(Request.Form, _faqRepo.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public IActionResult Create()
        {
            ViewData["FaqCategories"] = _faqCategoryRepo.GetDefaultQuery().Select(a => new SelectListItem() { Text = a.Title, Value = a.Id.ToString() }).ToList();

            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Faq model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _faqRepo.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var faq = await _faqRepo.GetById(id);
            ViewData["FaqCategories"] = _faqCategoryRepo.GetDefaultQuery().Select(a => new SelectListItem() { Text = a.Title, Value = a.Id.ToString(), Selected = (a.Id == faq.FaqCategoryId) }).ToList();

            return PartialView(faq);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Faq model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _faqRepo.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _faqRepo.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _faqRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
