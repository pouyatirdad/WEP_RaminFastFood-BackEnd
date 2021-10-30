using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface IFeaturesRepository : IBaseRepository<Feature>
    {
    }

    public class FeaturesRepository : BaseRepository<Feature>, IFeaturesRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public FeaturesRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
