using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface IOffersRepository : IBaseRepository<Offer>
    {
    }

    public class OffersRepository : BaseRepository<Offer>,IOffersRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public OffersRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
