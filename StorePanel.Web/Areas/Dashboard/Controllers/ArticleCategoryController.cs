using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class ArticleCategoryController : Controller
    {
        private readonly IArticleCategoryRepository _articleCategoryRepo;
        private readonly StorePanelDbContext _context;
        public ArticleCategoryController(IArticleCategoryRepository articleCategoryRepo, StorePanelDbContext context)
        {
            _articleCategoryRepo = articleCategoryRepo;
            _context = context;
        }
        public async Task<IActionResult> Index(bool root = false)
        {
            ViewBag.Root = root;
            var data = await _articleCategoryRepo.GetAll();
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public string LoadGrid()
        {
            var parser = new Parser<ArticleCategory>(Request.Form, _articleCategoryRepo.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public IActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ArticleCategory model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _articleCategoryRepo.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            return PartialView(await _articleCategoryRepo.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ArticleCategory model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _articleCategoryRepo.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _articleCategoryRepo.GetById(id));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _articleCategoryRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
