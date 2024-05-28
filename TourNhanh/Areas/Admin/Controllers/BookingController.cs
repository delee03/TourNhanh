using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;
using TourNhanh.Services.VnPay;
using TourNhanh.ViewModel;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBookingRepository _bookingRepository;
        private readonly IVnPayService _vnPayService;

        public BookingController(ITourRepository tourRepository, UserManager<AppUser> userManager, IBookingRepository bookingRepository, IVnPayService vnPayService)
        {
            _tourRepository = tourRepository;
            _userManager = userManager;
            _bookingRepository = bookingRepository;
            _vnPayService = vnPayService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _bookingRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
