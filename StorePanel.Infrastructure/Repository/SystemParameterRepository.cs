
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Repository
{
    public interface ISystemParameterRepository : IBaseRepository<SystemParameter>
    {
    }
    public class SystemParameterRepository : BaseRepository<SystemParameter>, ISystemParameterRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public SystemParameterRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
