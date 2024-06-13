using System.Diagnostics;
using System.Security.Cryptography.Pkcs;
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


        private readonly IConfiguration _configuration;
        public HomeController(ITourRepository tourRepository, ILogger<HomeController> logger, IConfiguration configuration)
        {
            _repository = tourRepository;
            _logger = logger;
            _configuration = configuration;
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



            var contactInfo = new ContactInfo
            {
                ZaloUrl = _configuration["ContactInfo:ZaloUrl"],
                PhoneUrl = _configuration["ContactInfo:PhoneUrl"],
                PhoneNumber = _configuration["ContactInfo:PhoneNumber"]
            };

            var viewModel = new HomeViewModel
            {
                ToursByRating = toursByRating,
                ToursByPopularity = toursByPopularity,          
                ToursByNewest = toursByNewest,
                ZaloUrl = contactInfo.ZaloUrl,
                PhoneUrl = contactInfo.PhoneUrl,
                PhoneNumber = contactInfo.PhoneNumber
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
