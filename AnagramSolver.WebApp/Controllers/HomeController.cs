using AnagramSolver.BusinessLogic;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        //public IActionResult Index()
        //{
        //    ViewBag.Message = "Hello from the controller";
        //    ViewBag.Name = "Mariia";

        //    return View();
        //}

        private readonly IAnagramSolver _anagramSolver;
        private readonly LetterCounter _letterCounter;

        public HomeController(IAnagramSolver anagramSolver, LetterCounter letterCounter) // DEPENDENCY INJECTION -- a dependency of homecontroller. homecontroller needs it to perform a job. Controller doesnt need to construct anything it just orders it to be created  builder.Services.AddScoped<IAnagramSolver, AnagramSolverService>(); - this tells asp.net to create a anagramSolverService 
        {
            _anagramSolver = anagramSolver;
            _letterCounter = letterCounter;
        }
        //"Dependency Injection is a design pattern where an object receives the dependencies it needs instead of creating them itself. In ASP.NET Core, the built-in DI container creates and injects those dependencies based on the registrations in Program.cs."
        //"Constructor injection makes the controller depend on abstractions rather than concrete implementations. The controller only receives the services it needs and focuses on coordinating the request. The DI container is responsible for creating and supplying those services."
        public IActionResult Index(string? id)
        {
            var model = new AnagramViewModel()
            {
                Input = id
            };
            if(!string.IsNullOrWhiteSpace(id))
            {
                var idToDictionary = _letterCounter.CountLetters(id);
                model.Anagrams = _anagramSolver.GetAnagrams(idToDictionary);
            }
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
