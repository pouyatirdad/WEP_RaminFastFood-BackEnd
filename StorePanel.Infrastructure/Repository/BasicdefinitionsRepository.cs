using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Repository
{


    public interface IBasicdefinitionsRepository : IBaseRepository<Basicdefinitions>
    {

    }
    public class BasicdefinitionsRepository : BaseRepository<Basicdefinitions>, IBasicdefinitionsRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public BasicdefinitionsRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    } 
}
