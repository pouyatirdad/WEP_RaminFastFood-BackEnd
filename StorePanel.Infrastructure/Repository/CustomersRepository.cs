using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Core.Utility;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface ICustomersRepository : IBaseRepository<Customer>
    {
        List<Customer> GetCustomerTable();

        Customer GetCustomer(int id);
        Customer GetUserCustomer(String id);

        Customer GetInvoicesCustomer(String userid);
    }

    public class CustomersRepository : BaseRepository<Customer>, ICustomersRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public CustomersRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Customer> GetCustomerTable()
        {
            return _context.Customers.Where(c => c.IsDeleted == false).Include(c => c.User).ToList();
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.Include(c => c.User).FirstOrDefault(c => c.Id == id);
        }
        public Customer GetUserCustomer(String id)
        {
            return _context.Customers.Include(c => c.User).FirstOrDefault(c => c.UserId == id);
        }

        public Customer GetInvoicesCustomer(String userid)
        {

            var p = _context.Customers.Where(a => a.UserId == userid).Include(c => c.User).FirstOrDefault();
            return p;
        }
    }
}
