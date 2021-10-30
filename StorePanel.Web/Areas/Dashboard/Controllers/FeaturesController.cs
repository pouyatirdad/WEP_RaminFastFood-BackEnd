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
    public class FeaturesController : Controller
    {
        private readonly IFeaturesRepository _featuresRepository;

        public FeaturesController(IFeaturesRepository featuresRepository)
        {
            _featuresRepository = featuresRepository;
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
            var parser = new Parser<Feature>(Request.Form, _featuresRepository.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public IActionResult Create()
        {
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Feature feature)
        {
            if (ModelState.IsValid)
            {
                await _featuresRepository.AddOrUpdate(feature);
                return RedirectToAction("Index");
            }

            return View(feature);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Feature feature =await _featuresRepository.GetById(id.Value);
            if (feature == null)
            {
                return NotFound();
            }
            return PartialView(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Feature feature)
        {
            if (ModelState.IsValid)
            {
                await _featuresRepository.Update(feature);
                return RedirectToAction("Index");
            }
            return View(feature);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Feature feature =await _featuresRepository.GetById(id.Value);
            if (feature == null)
            {
                return NotFound();
            }
            return PartialView(feature);
        }

        // POST: Admin/Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _featuresRepository.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
