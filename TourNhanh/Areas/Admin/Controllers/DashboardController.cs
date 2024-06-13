using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBookingRepository bookingRepository;
        public DashboardController(UserManager<AppUser> userManager, IBookingRepository _bookingRepository)
        {
            _userManager = userManager;
            bookingRepository = _bookingRepository;
        }

        public async Task<IActionResult> Index () 
        {
            var totalBookingsCount = await bookingRepository.GetTotalBookingsCountAsync();
            var totalRevenue = await bookingRepository.GetTotalRevenueAsync();
            // Get a list of all customers
            var User = await _userManager.GetUsersInRoleAsync("Customer");

            ViewBag.UserCount = User.Count();

            ViewBag.TotalBookingsCount = totalBookingsCount;
            ViewBag.TotalRevenue = totalRevenue;
            return View();
        }
    }
}
