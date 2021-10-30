using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Repository
{
    public interface IStaticContentRepository : IBaseRepository<StaticContent>
    {
    }
    public class StaticContentRepository : BaseRepository<StaticContent>, IStaticContentRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public StaticContentRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
