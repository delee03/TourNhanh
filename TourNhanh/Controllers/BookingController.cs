using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;
using TourNhanh.Services.VnPay;
using TourNhanh.ViewModel;

namespace TourNhanh.Controllers
{
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
        [Authorize]
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
            if (userId == null)
            {
                return Unauthorized();
            }

            var booking = new Booking
            {
                TourId = tourId,
                BookingDate = DateTime.Now,
                Quantity = quantity,
                Amount = quantity * tour.Price,
                CustomerUserId = userId,
                /*  CustomerUserId = null,*///Test, chạy thật thì lấy cái trên
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
            return RedirectToAction("YourTour", new { bookingId = booking.Id });
        }


        [HttpGet]
        public async Task<IActionResult> ProcessPayment(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            //var userId = _userManager.GetUserId(User);
            var user = await _userManager.GetUserAsync(User);

            ViewBag.FullName = user.FullName;
            ViewBag.Email = user.Email ;
            ViewBag.Phone = user.PhoneNumber;
            ViewBag.BookingId = bookingId;
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPaymentConfirmed(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var user = await _userManager.GetUserAsync(User);

            /*if (booking.PaymentMethod == "online")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = booking.Amount,
                    CreatedDate = DateTime.Now,
                    Desc =*//* $"{user.FullName} {user.PhoneNumber}"*//* "",
                    FullName = *//*user.FullName*//* "",
                    BookingId = bookingId,
                    ReturnUrl = Url.Action("PaymentCallBack", "Booking", bookingId, Request.Scheme)  // URL callback

                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }*/

            var vnPayModel = new VnPaymentRequestModel
            {
                Amount = (double)booking.Amount ,
                CreatedDate = DateTime.Now,
                Desc = $"{user.FullName} {user.PhoneNumber}",
                FullName = user.FullName ,
                BookingId = bookingId,
                PaymentBackReturnUrl = Url.Action("PaymentCallBack", "Booking", bookingId, Request.Scheme)  // URL callback
            };
            if (booking != null)
            {
                booking.PaymentMethod = "online";
                booking.PaymentDate = DateTime.Now;
                booking.isPaymentCompleted = true;
                await _bookingRepository.UpdateAsync(booking);
            }
            return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            
            /*return RedirectToAction("Details", new { bookingId = booking.Id });*/
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

        //[Authorize]
        public async Task<IActionResult> YourTour()
        {
            var user = await _userManager.GetUserAsync(User);
            // var userId = _userManager.GetUserId(User);         
            /*if(userId == null)
            {
                return NotFound();
            }*/
            //ViewBag.UserName = userName;
            ViewBag.FullName = user.FullName;
			var userBooking = await _bookingRepository.GetUserTour(user.Id);
            return View(userBooking);
        }

        /*[Authorize]*/
        public IActionResult PaymentFail()
        {
            return View();
        }
        /*[Authorize]*/
        public async Task<IActionResult> PaymentCallBack(int bookingId)
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }
            //Lưu đơn hàng vào database tự code
            TempData["Message"] = $"Thanh toán VNPAY thành công";



            return View("SuccessfulOnlinePay");
        }
    }
}
