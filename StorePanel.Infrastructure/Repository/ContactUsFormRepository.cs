using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Repository
{
    public interface IContactUsFormRepository : IBaseRepository<ContactForm>
    {
    }
    public class ContactUsFormRepository : BaseRepository<ContactForm>, IContactUsFormRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public ContactUsFormRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
