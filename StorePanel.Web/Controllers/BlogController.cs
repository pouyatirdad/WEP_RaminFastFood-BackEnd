using Microsoft.AspNetCore.Mvc;
using StorePanel.Core.Models;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Controllers
{
    public class BlogController : Controller
    {

        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCommentRepository _articleCommentRepository;

        public BlogController(IArticleRepository articleRepository, IArticleCommentRepository articleCommentRepository)
        {
            _articleRepository = articleRepository;
            _articleCommentRepository = articleCommentRepository;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 9, int? BlogGroupId = null)
        {

            ViewBag.PageID = page;
            ViewBag.PageSize = pageSize;

            var data = new List<Article> { };

            if (BlogGroupId == null)
            {
                data = await _articleRepository.GetAll();
            }
            else
            {
                data =  _articleRepository.GetByBlogGroup(BlogGroupId.Value);
            }

            ViewBag.All = data.Count();

            return View(data);
        }

        public async Task<IActionResult> Detail(int Id)
        {
            var model = await _articleRepository.GetById(Id);
            model.ArticleComments = _articleCommentRepository.GetCmByArticleId(Id).Result;
            //model.ArticleTags = _articleRepository.GetArticleTags(Id).Result;
            return View(model);
        }

        public IActionResult Search(int page = 1, int pageSize = 9,string ws=null)
        {
            ViewBag.PageID = page;
            ViewBag.PageSize = pageSize;

            var data = _articleRepository.SearchArticle(ws);

            ViewBag.All = data.Count();

            return View("Index",data);
        }
        [HttpPost]
        public string SendComment(ArticleComment comment)
        {


            if (ModelState.IsValid)
            {
                comment.AddedDate = DateTime.Now;
                _articleCommentRepository.Add(comment);
                return "success";
            }
            return "fail";
        }
    }
}
