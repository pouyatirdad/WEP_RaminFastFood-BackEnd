using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface IGeoDivisionsRepository : IBaseRepository<GeoDivision>
    {
        List<GeoDivision> GetGeoDivisionsByType(int type);
    }

    public class GeoDivisionsRepository : BaseRepository<GeoDivision>, IGeoDivisionsRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;
        public GeoDivisionsRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<GeoDivision> GetGeoDivisionsByType(int type)
        {
            return _context.GeoDivisions.Where(g => g.IsDeleted == false && g.GeoDivisionType == type).ToList();
        }
    }
}
