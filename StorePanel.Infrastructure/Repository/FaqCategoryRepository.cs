using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Repository
{
    public interface IFaqCategoryRepository : IBaseRepository<FaqCategory>
    {
    }
    public class FaqCategoryRepository : BaseRepository<FaqCategory>, IFaqCategoryRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public FaqCategoryRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
