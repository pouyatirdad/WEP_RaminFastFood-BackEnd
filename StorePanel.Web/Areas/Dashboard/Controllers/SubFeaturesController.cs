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
    public class SubFeaturesController : Controller
    {
        private readonly ISubFeaturesRepository _subFeaturesRepository;

        public SubFeaturesController(ISubFeaturesRepository subFeaturesRepository)
        {
            _subFeaturesRepository = subFeaturesRepository;
        }

        public IActionResult Index(int id,bool root = false)
        {
            ViewBag.Root = root;
            ViewBag.FeatureId = id;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public string LoadGrid(int Featureid)
        {
            var parser = new Parser<SubFeature>(Request.Form, _subFeaturesRepository.GetDefaultQuery().Where(x=>x.FeatureId== Featureid).AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public IActionResult Create(int Featureid)
        {
            ViewBag.featureId = Featureid;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubFeature subFeature)
        {
            if (ModelState.IsValid)
            {
                await _subFeaturesRepository.AddOrUpdate(subFeature);
                return RedirectToAction("Index",new { id=subFeature.FeatureId });
            }

            return View(subFeature);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SubFeature feature = await _subFeaturesRepository.GetById(id.Value);
            if (feature == null)
            {
                return NotFound();
            }
            return PartialView(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubFeature subFeature)
        {
            if (ModelState.IsValid)
            {
                _subFeaturesRepository.Update(subFeature);
                return RedirectToAction("Index");
            }
            return View(subFeature);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SubFeature feature = await _subFeaturesRepository.GetById(id.Value);
            if (feature == null)
            {
                return NotFound();
            }
            return PartialView(feature);
        }

        // POST: Admin/Features/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _subFeaturesRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
