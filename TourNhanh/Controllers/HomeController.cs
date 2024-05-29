using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITourRepository _repository;
        public HomeController(ITourRepository tourRepository)
        {
            _repository = tourRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tour = await _repository.GetAllAsync();
            return View(tour);
        }
        public IActionResult Test()
        {
            return View();
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
