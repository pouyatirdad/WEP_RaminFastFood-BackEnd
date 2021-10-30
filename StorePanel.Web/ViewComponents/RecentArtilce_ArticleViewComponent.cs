using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class RecentArtilce_ArticleViewComponent :ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public RecentArtilce_ArticleViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _articleRepository.GetAll());
        }
    }
}
