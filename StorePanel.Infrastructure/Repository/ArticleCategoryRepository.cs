using System;
using System.Collections.Generic;
using System.Text;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;

namespace StorePanel.Infrastructure.Repository
{
    public interface IArticleCategoryRepository : IBaseRepository<ArticleCategory>
    {
    }
    public class ArticleCategoryRepository : BaseRepository<ArticleCategory>, IArticleCategoryRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly ILogRepository _logger;

        public ArticleCategoryRepository(StorePanelDbContext context, ILogRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
