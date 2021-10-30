using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class DiscountController : Controller
    {
        private readonly IDiscountsRepository _repo;
        private readonly IOffersRepository _offerRepo;
        private readonly IBrandsRepository _brandRepo;
        private readonly IProductGroupsRepository _productGroupRepo;
        private readonly IProductsRepository _productRepo;

        public DiscountController(IDiscountsRepository discountsRepository
        ,IOffersRepository offersRepository
        ,IBrandsRepository brandsRepository
        ,IProductGroupsRepository productGroupsRepository
        ,IProductsRepository productsRepository)
        {
            _repo = discountsRepository;

            _offerRepo = offersRepository;

            _brandRepo = brandsRepository;

            _productGroupRepo = productGroupsRepository;

            _productRepo = productsRepository;
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
            var parser = new Parser<Discount>(Request.Form, _repo.GetDefaultQuery().AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Offers"] = new SelectList(await _offerRepo.GetAll(), "Id", "Title");
            ViewData["Brands"] = new SelectList(await _brandRepo.GetAll(), "Id", "Name");
            ViewData["ProductGroups"] = new SelectList(await _productGroupRepo.GetAll(), "Id", "Title");
            ViewData["Products"] = new SelectList(await _productRepo.GetAll(), "Id", "Title");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DiscountFormViewModel newDiscount)
        {
            if (ModelState.IsValid)
            {
                var groupIdentifier = Guid.NewGuid().ToString();
                #region Adding Brands Discounts
                if (newDiscount.BrandIds != null)
                {
                    foreach (var item in newDiscount.BrandIds)
                    {
                        var discount = new Discount()
                        {
                            DiscountType = newDiscount.DiscountType,
                            Amount = newDiscount.Amount,
                            OfferId = newDiscount.OfferId,
                            Title = newDiscount.Title,
                            BrandId = item,
                            GroupIdentifier = groupIdentifier
                        };
                        await _repo.Add(discount);
                    }
                }
                #endregion
                #region Adding ProductGroups Discounts
                if (newDiscount.ProductGroupIds != null)
                {
                    foreach (var item in newDiscount.ProductGroupIds)
                    {
                        var discount = new Discount()
                        {
                            DiscountType = newDiscount.DiscountType,
                            Amount = newDiscount.Amount,
                            OfferId = newDiscount.OfferId,
                            Title = newDiscount.Title,
                            ProductGroupId = item,
                            GroupIdentifier = groupIdentifier
                        };
                        await _repo.Add(discount);
                    }
                }
                #endregion

                #region Adding Products Discounts
                if (newDiscount.ProductIds != null)
                {
                    foreach (var item in newDiscount.ProductIds)
                    {
                        var discount = new Discount()
                        {
                            DiscountType = newDiscount.DiscountType,
                            Amount = newDiscount.Amount,
                            OfferId = newDiscount.OfferId,
                            Title = newDiscount.Title,
                            ProductId = item,
                            GroupIdentifier = groupIdentifier
                        };
                         await _repo.Add(discount);
                    }
                }
                #endregion

                return RedirectToAction("Index");
            }
            ViewData["Offers"] = new SelectList(await _offerRepo.GetAll(), "Id", "Title");
            ViewData["Brands"] = new SelectList(await _brandRepo.GetAll(), "Id", "Name");
            ViewData["ProductGroups"] = new SelectList(await _productGroupRepo.GetAll(), "Id", "Title");
            ViewData["Products"] = new SelectList(await _productRepo.GetAll(), "Id", "Title");
            return View();
        }

        public IActionResult Edit(int id)
        {
            #region Edit Props

            var vm = new DiscountFormViewModel();
            var discountGroup = _repo.GetDiscountGroup(id);
            var groupIdentifier = discountGroup.FirstOrDefault().GroupIdentifier;
            vm.PreviousDiscounts = discountGroup;
            vm.GroupIdentifier = groupIdentifier;
            vm.Title = discountGroup.FirstOrDefault().Title;
            vm.OfferId = discountGroup.FirstOrDefault().OfferId;
            vm.DiscountType = discountGroup.FirstOrDefault().DiscountType;
            vm.Amount = discountGroup.FirstOrDefault().Amount;

            #endregion

            ViewBag.Offers = _offerRepo.GetAll();
            ViewBag.Brands = _brandRepo.GetAll();
            ViewBag.ProductGroups = _productGroupRepo.GetAll();
            ViewBag.Products = _productRepo.GetAll();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DiscountFormViewModel newDiscount)
        {
            if (ModelState.IsValid)
            {
                #region Removing All Previous Discounts
                var prevDicounts = _repo.GetDiscountsByGroupIdentifier(newDiscount.GroupIdentifier);

                foreach (var item in prevDicounts)
                    await _repo.Delete(item.Id);

                #endregion

                var groupIdentifier = Guid.NewGuid().ToString();
                #region Adding Brands Discounts
                if (newDiscount.BrandIds != null)
                {
                    foreach (var item in newDiscount.BrandIds)
                    {
                        var discount = new Discount()
                        {
                            DiscountType = newDiscount.DiscountType,
                            Amount = newDiscount.Amount,
                            OfferId = newDiscount.OfferId,
                            Title = newDiscount.Title,
                            BrandId = item,
                            GroupIdentifier = groupIdentifier
                        };
                        await _repo.Add(discount);
                    }
                }
                #endregion
                #region Adding ProductGroups Discounts
                if (newDiscount.ProductGroupIds != null)
                {
                    foreach (var item in newDiscount.ProductGroupIds)
                    {
                        var discount = new Discount()
                        {
                            DiscountType = newDiscount.DiscountType,
                            Amount = newDiscount.Amount,
                            OfferId = newDiscount.OfferId,
                            Title = newDiscount.Title,
                            ProductGroupId = item,
                            GroupIdentifier = groupIdentifier
                        };
                        await _repo.Add(discount);
                    }
                }
                #endregion

                #region Adding Products Discounts
                if (newDiscount.ProductIds != null)
                {
                    foreach (var item in newDiscount.ProductIds)
                    {
                        var discount = new Discount()
                        {
                            DiscountType = newDiscount.DiscountType,
                            Amount = newDiscount.Amount,
                            OfferId = newDiscount.OfferId,
                            Title = newDiscount.Title,
                            ProductId = item,
                            GroupIdentifier = groupIdentifier
                        };
                        await _repo.Add(discount);
                    } 
                }
                #endregion

                return RedirectToAction("Index");
            }

            ViewData["Offers"] = new SelectList(await _offerRepo.GetAll(), "Id", "Title");
            ViewData["Brands"] = new SelectList(await _brandRepo.GetAll(), "Id", "Name");
            ViewData["ProductGroups"] = new SelectList(await _productGroupRepo.GetAll(), "Id", "Title");
            ViewData["Products"] = new SelectList(await _productRepo.GetAll(), "Id", "Title");
            return View();
        }


        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _repo.GetById(id));
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
