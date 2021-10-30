using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Repository
{
    public interface IFaqRepository : IBaseRepository<Faq>
    {
    }
    public class FaqRepository : BaseRepository<Faq>, IFaqRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public FaqRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
