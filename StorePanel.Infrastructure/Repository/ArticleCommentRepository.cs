using Microsoft.EntityFrameworkCore;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorePanel.Infrastructure.Repository
{
    public interface IArticleCommentRepository : IBaseRepository<ArticleComment>
    {
        Task<List<ArticleComment>> GetCmByArticleId(int articleId);

    }
    public class ArticleCommentRepository : BaseRepository<ArticleComment>, IArticleCommentRepository
    {
        private readonly StorePanelDbContext _context;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly ILogRepository _logger;

        public ArticleCommentRepository(StorePanelDbContext context, ILogRepository logger, IArticleCategoryRepository articleCategoryRepository) : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _articleCategoryRepository = articleCategoryRepository;
        }

        public async Task<List<ArticleComment>> GetCmByArticleId(int articleId)
        {

            var cms =  _context.ArticleComments.Where(x => x.IsDeleted == false && x.IsShow == true && x.ArticleId == articleId).ToList();
            return cms;
        }
    }
}
