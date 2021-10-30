using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class ArtilcetagsViewComponent : ViewComponent
    {
        private readonly IArticleRepository _articleRepository;

        public ArtilcetagsViewComponent(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IViewComponentResult Invoke()
        {
            return View(_articleRepository.GetsomeArtilceTags(9));
        }
    }
}
