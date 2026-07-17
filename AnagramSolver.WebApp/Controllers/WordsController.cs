using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace AnagramSolver.WebApp.Controllers;

    public class WordsController : Controller
    {
        private const int PageSize = 100;
        private readonly IWordRepository _wordRepository;

        public WordsController(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        } // constructor
        //    The controller declares
        //I require an object that fulfils the IWordRepository contract
        //ASP.NET obtains the registered implementation from the DI container and supplies it to the constructor.

        public IActionResult Index(int page = 1)
        {
           var allWords = _wordRepository.GetAllWords();
           if(page < 1)
           {
            page = 1;
           }
  
            int totalPages = (int)Math.Ceiling(
            allWords.Length / (double)PageSize);

            if(totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }

            var wordsForCurrentPage = allWords
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

