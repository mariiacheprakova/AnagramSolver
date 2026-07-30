using System.Diagnostics;
using System.Text.Json;
using AnagramSolver.BusinessLogic;
using AnagramSolver.Contracts;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly LetterCounter _letterCounter;

        public HomeController(IAnagramSolver anagramSolver, LetterCounter letterCounter)
        {
            _anagramSolver = anagramSolver;
            _letterCounter = letterCounter;
        }

        public async Task<IActionResult> Index(string? id, CancellationToken cancellationToken)
        {
            var model = new AnagramViewModel() { Input = id };

            var historyJson = HttpContext.Session.GetString("searchHistory");
            var searchHistory = string.IsNullOrWhiteSpace(historyJson)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(historyJson) ?? new List<string>();

            if (!string.IsNullOrWhiteSpace(id))
            {
                Response.Cookies.Append(
                    "lastSearch",
                    id,
                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) }
                );

                searchHistory.Add(id);
                var updatedHistoryJson = JsonSerializer.Serialize(searchHistory);
                HttpContext.Session.SetString("searchHistory", updatedHistoryJson);

                var idToDictionary = _letterCounter.CountLetters(id);
                model.Anagrams = await _anagramSolver.GetAnagramsAsync(
                    idToDictionary,
                    cancellationToken
                );
            }

            var lastSearch = Request.Cookies["lastSearch"];
            ViewBag.LastSearch = lastSearch;
            ViewBag.SearchHistory = searchHistory;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                }
            );
        }
    }
}
