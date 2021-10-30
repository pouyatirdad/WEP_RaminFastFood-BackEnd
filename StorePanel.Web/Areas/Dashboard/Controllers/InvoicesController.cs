using DataTablesParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Permission")]
    public class InvoicesController : Controller
    {
        private readonly IInvoicesRepository _invoicesRepository;

        private readonly IGeoDivisionsRepository _geoDivisionsRepository;

        public InvoicesController(IInvoicesRepository invoicesRepository,IGeoDivisionsRepository geoDivisionsRepository)
        {
            _invoicesRepository = invoicesRepository;

            _geoDivisionsRepository = geoDivisionsRepository;
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
            var parser = new Parser<Invoice>(Request.Form, _invoicesRepository.GetDefaultQuery().OrderByDescending(x=>x.Id).AsQueryable());
            return JsonConvert.SerializeObject(parser.Parse());
        }

        public async Task<IActionResult> Delete(int id)
        {
            return PartialView(await _invoicesRepository.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _invoicesRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Invoice invoice =await _invoicesRepository.GetById(id.Value);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewBag.GeoDivisionId = new SelectList(_geoDivisionsRepository.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", invoice.GeoDivisionId);
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _invoicesRepository.Update(invoice);
                return RedirectToAction("Index");
            }
            ViewBag.GeoDivisionId = new SelectList(_geoDivisionsRepository.GetGeoDivisionsByType((int)GeoDivisionType.State), "Id", "Title", invoice.GeoDivisionId);
            return View(invoice);
        }

        public IActionResult Detail(int Id)
        {
            var vm = new ViewInvoiceViewModel();
            var invoice = _invoicesRepository.GetInvoice(Id);
            vm.Invoice = invoice;
            vm.PersianDate = new PersianDateTime(invoice.AddedDate).ToString();
            vm.InvoiceItems = new List<InvoiceItemWithMainFeatureViewModel>();
            // Getting Invoice Item SubFeatures
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                var invoiceItemWithMainFeature = new InvoiceItemWithMainFeatureViewModel
                {
                    InvoiceItem = invoiceItem,
                    MainFeature = _invoicesRepository.GetInvoiceItemsMainFeature(invoiceItem.Id)
                };
                vm.InvoiceItems.Add(invoiceItemWithMainFeature);

            }
            return View(vm);
        }
    }
}
