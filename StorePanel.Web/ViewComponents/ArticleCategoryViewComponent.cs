using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.ViewComponents
{
    public class ArticleCategoryViewComponent : ViewComponent
    {
        private readonly IArticleCategoryRepository _articleCategoryRepository;

        public ArticleCategoryViewComponent(IArticleCategoryRepository articleCategoryRepository)
        {
            _articleCategoryRepository = articleCategoryRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _articleCategoryRepository.GetAll());
        }
    }
}
