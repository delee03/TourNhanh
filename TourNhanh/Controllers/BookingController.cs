using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create(int tourId)
        {
            var tour = await _tourRepository.GetByIdAsync(tourId);
            if (tour == null)
            {
                return NotFound();
            }
            ViewBag.TourId = tourId;
            ViewBag.Price = tour.Price;
            ViewBag.RemainingSlots = tour.RemainingSlots;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int tourId, int quantity, string paymentMethod, string note)
        {
            var tour = await _tourRepository.GetByIdAsync(tourId);

            if (tour == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            //Chạy thật xóa comment đoạn dưới
            /*if (userId == null)
            {
                return Unauthorized();
            }*/

            var booking = new Booking
            {
                TourId = tourId,
                BookingDate = DateTime.Now,
                Quantity = quantity,
                Amount = quantity * tour.Price,
                /*CustomerUserId = userId,*/
                CustomerUserId = null,//Test, chạy thật thì lấy cái trên
                PaymentDate = null,
                PaymentMethod = paymentMethod,
                Note = note,
                ContactPersonUserId = null,
                isPaymentCompleted = false
            };

            // Save the booking to the database
            await _bookingRepository.CreateAsync(booking);
            tour.RemainingSlots = tour.RemainingSlots - booking.Quantity;
            await _tourRepository.UpdateAsync(tour);

            // Redirect to the payment processing page
            return RedirectToAction("Details", new { bookingId = booking.Id });
        }

        public async Task<IActionResult> ProcessPayment(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // CODE THANH TOÁN Ở ĐÂY

            //CODE THANH TOÁN NHỚ UPDATE PAYMENTDATE VỚI IS PAYMENT COMPLETE

            return View(booking);
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
