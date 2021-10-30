using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class MostViewArtilceViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public MostViewArtilceViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IViewComponentResult Invoke()
        {
            return View(_articleRepository.GetMostViewArtilce(4));
        }
    }
}
