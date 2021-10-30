using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class RecentArtilcel_IndexViewComponent :ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public RecentArtilcel_IndexViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IViewComponentResult Invoke()
        {
            return View( _articleRepository.GetSomeArticle(6));
        }
    }
}
