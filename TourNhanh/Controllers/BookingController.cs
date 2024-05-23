using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourNhanh.Extensions;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Controllers
{
    public class BookingController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBookingRepository _bookingRepository;

        public BookingController(ITourRepository tourRepository, UserManager<AppUser> userManager, IBookingRepository bookingRepository)
        {
            _tourRepository = tourRepository;
            _userManager = userManager;
            _bookingRepository = bookingRepository;
        }

        [HttpGet]
        public IActionResult Create(int tourId)
        {
            // Lấy thông tin tour từ database
            var tour = _tourRepository.GetByIdAsync(tourId).Result;

            if (tour == null)
            {
                return NotFound();
            }
            return View();
        }


        [HttpPost]
        public IActionResult Create(int tourId, int quantity, string paymentMethod,string note)
        {
            // Lấy thông tin tour từ database
            var tour = _tourRepository.GetByIdAsync(tourId).Result;

            if (tour == null)
            {
                return NotFound();
            }

            // Lấy thông tin người dùng hiện tại
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return Unauthorized();
            }

            // Tạo một đối tượng Booking mới
            var booking = new Booking
            {
                TourId = tourId,
                BookingDate = DateTime.Now,
                Quantity = quantity,
                Amount = quantity * tour.Price,
                CustomerUserId = userId,
                PaymentDate = DateTime.Now, // Đặt ngày thanh toán hiện tại hoặc khi người dùng thanh toán
                PaymentMethod = paymentMethod,
                Note=note,
                ContactPersonUserId=null
                // Bạn có thể thêm các trường khác của booking ở đây
            };

            // Lưu booking vào session
            HttpContext.Session.SetObjectAsJson("CurrentBooking", booking);

            // Chuyển hướng đến view tạo booking thành công hoặc hiển thị thông tin booking
            return RedirectToAction("BookingDetails");
        }

        public IActionResult BookingDetails(int bookingId)
        {
            // Lấy booking từ session
            var booking = HttpContext.Session.GetObjectFromJson<Booking>("CurrentBooking");

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }
}
