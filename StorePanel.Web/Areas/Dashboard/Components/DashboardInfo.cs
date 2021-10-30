using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using StorePanel.Web.Areas.Dashboard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Areas.Dashboard.Components
{
    public class DashboardInfo : ViewComponent
    {
        private readonly ICustomersRepository _customersRepository;

        private readonly IInvoicesRepository _invoicesRepository;

        private readonly IProductsRepository _productsRepository;

        public DashboardInfo(ICustomersRepository customersRepository
        ,IInvoicesRepository invoicesRepository
        ,IProductsRepository productsRepository)
        {
            _customersRepository = customersRepository;

            _invoicesRepository = invoicesRepository;

            _productsRepository = productsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            DashboardInfoViewModel dashboardInfo = new DashboardInfoViewModel()
            {
                CustomerCount=await _customersRepository.GetCount(),
                InvoiceCount=await _invoicesRepository.GetCount(),
                ProductCount=await _productsRepository.GetCount(),
                TotalSell=_invoicesRepository.GetTotalSell()
            };

            return View(dashboardInfo);
        }
    }
}
