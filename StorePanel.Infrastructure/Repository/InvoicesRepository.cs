using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Infrastructure.Repository
{
    public interface IInvoicesRepository : IBaseRepository<Invoice>
    {
        List<InvoiceItem> GetInvoiceItems(int InvoicId);

        List<Invoice> GetInvoices();
        List<Invoice> GetInvoicesCustomer(int customerid);

        Invoice GetInvoice(int invoiceId);

        string GetInvoiceItemsMainFeature(int invoiceItemId);
        List<Product> GertTopSoldProducts(int take);

        List<Invoice> GetCustomerInvoices(int customerId);

        InvoiceItem AddInvoiceItem(InvoiceItem invoiceItem, User user);

         List<InvoiceItem> GetInvoiceItemsByInvoiceId(int id);

         Invoice GetInvoiceWithGeo(int id);

        long GetTotalSell();
    }

    public class InvoicesRepository : BaseRepository<Invoice>, IInvoicesRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public InvoicesRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;

        }
        public List<InvoiceItem> GetInvoiceItems(int InvoicId)
        {
            var InvoiceItems = _context.InvoiceItems.Include(a => a.Product).Include(a => a.Product.ProductMainFeatures).Where(a => a.InvoiceId == InvoicId).ToList();
            return InvoiceItems;
        }

        public List<Invoice> GetInvoices()
        {
            return _context.Invoices.Include(i => i.Customer.User).ToList();
        }
        public List<Invoice> GetInvoicesCustomer(int customerid)
        {
            return _context.Invoices.Include(i => i.Customer.User).Where(a => a.CustomerId == customerid).ToList();
        }

        public Invoice GetInvoice(int invoiceId)
        {
            return _context.Invoices.Include(i => i.Customer.User).Include(i => i.InvoiceItems).FirstOrDefault(i => i.Id == invoiceId);
        }

        public string GetInvoiceItemsMainFeature(int invoiceItemId)
        {
            var invoiceItem = _context.InvoiceItems.Find(invoiceItemId);
            var mainFeature = _context.ProductMainFeatures.Include(m => m.SubFeature).Include(m => m.Product).FirstOrDefault(m => m.Id == invoiceItem.MainFeatureId);
            return mainFeature.SubFeature.Value;
        }
        public List<Product> GertTopSoldProducts(int take)
        {
            List<Product> products = new List<Product>();
            var productIds = _context.InvoiceItems.GroupBy(i => i.ProductId)
                .OrderByDescending(pi => pi.Count())
                .Select(g => g.Key).ToList();
            foreach (var id in productIds)
            {
                if (products.Count < take)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Id == id);
                    if (product != null && product.IsDeleted == false)
                    {
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public List<Invoice> GetCustomerInvoices(int customerId)
        {
            return _context.Invoices.Where(i => i.IsDeleted == false && i.CustomerId == customerId).ToList();
        }

        public InvoiceItem AddInvoiceItem(InvoiceItem invoiceItem, User user)
        {

            invoiceItem.InsertDate = DateTime.Now;
            if (user != null)
                invoiceItem.InsertUser = user.UserName;

            _context.InvoiceItems.Add(invoiceItem);
            _context.SaveChanges();

            return invoiceItem;
        }

        public List<InvoiceItem> GetInvoiceItemsByInvoiceId(int id)
        {
            return _context.InvoiceItems.Include(i => i.Product).Where(i => i.IsDeleted == false && i.InvoiceId == id)
                .ToList();
        }
        public Invoice GetInvoiceWithGeo(int id)
        {
            return _context.Invoices.Include(i => i.GeoDivision).FirstOrDefault(i => i.Id == id);
        }

        public long GetTotalSell() {
            long total = 0;
            var payedInvoices=_context.Invoices.Where(x => x.IsPayed == true).ToList();
            foreach (var item in payedInvoices)
            {
                total += item.TotalPrice;
            }
            return total;
        }
    }


}
