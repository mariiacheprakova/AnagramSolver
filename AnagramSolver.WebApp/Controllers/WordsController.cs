using AnagramSolver.BusinessLogic;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace AnagramSolver.WebApp.Controllers;

    public class WordsController : Controller
    {
        private const int PageSize = 100;
        private readonly IWordRepository _wordRepository;

        public WordsController(IWordRepository FileWordRepository)
        {
            _wordRepository = FileWordRepository;
        }


        public IActionResult Index(int page = 1)
        {
           if(page < 1)
           {
            page = 1;
           }
            int totalPages = (int)Math.Ceiling(
            Words.Count / (double)PageSize);

            var wordsForCurrentPage = Words
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToArray();

            var model = new WordsViewModel
            {
                Words = wordsForCurrentPage,
                CurrentPage = page,
                TotalPages = totalPages
            };

           return View(model);
        }
    }

