using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePanel.Infrastructure.Repository
{

    public interface ISliderRepository : IBaseRepository<Slider>
    {
    }
    public class SliderRepository : BaseRepository<Slider>, ISliderRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public SliderRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
