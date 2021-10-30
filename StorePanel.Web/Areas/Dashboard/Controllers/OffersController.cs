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
    public class OffersController : Controller
    {
        private readonly IOffersRepository _offersRepository;

        public OffersController(IOffersRepository offersRepository)
        {
            _offersRepository = offersRepository;
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
            var parser = new Parser<Offer>(Request.Form, _offersRepository.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile BrandImage, Offer model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            await _offersRepository.AddOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Offer offer =await _offersRepository.GetById(id.Value);
            if (offer == null)
            {
                return NotFound();
            }
            return PartialView(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Offer offer, IFormFile OfferImage)
        {
            if (ModelState.IsValid)
            {
                #region Upload Image
                if (OfferImage != null)
                {
                    //Upload Image
                }
                #endregion

                _offersRepository.Update(offer);
                return RedirectToAction("Index");
            }
            return View(offer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Offer offer =await _offersRepository.GetById(id.Value);
            if (offer == null)
            {
                return NotFound();
            }
            return PartialView(offer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer =await _offersRepository.GetById(id);

            await _offersRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
