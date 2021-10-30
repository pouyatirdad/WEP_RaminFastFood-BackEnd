using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePanel.Web.Controllers
{
    public class FaqController : Controller
    {
        private readonly IFaqRepository _faqRepository;

        private readonly IFaqCategoryRepository _faqCategoryRepository;

        public FaqController(IFaqRepository faqRepository
        ,IFaqCategoryRepository faqCategoryRepository)
        {
            _faqRepository = faqRepository;

            _faqCategoryRepository = faqCategoryRepository;
        }

        public IActionResult Index()
        {
            return View(_faqRepository.GetAll());
        }
    }
}
