using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;
using TourNhanh.ViewModel;


namespace TourNhanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITourRepository _repository;
        public HomeController(ITourRepository tourRepository, ILogger<HomeController> logger)
        {
            _repository = tourRepository;
            _logger = logger;
        }

        public IActionResult Test()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var toursByRating = await _repository.GetSortedToursAsync("rating");
            var toursByPopularity = await _repository.GetSortedToursAsync("popular");
            var toursByNewest = await _repository.GetSortedToursAsync("newest");

            var viewModel = new HomeViewModel
            {
                ToursByRating = toursByRating,
                ToursByPopularity = toursByPopularity,
                ToursByNewest = toursByNewest
            };

            return View(viewModel);
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
