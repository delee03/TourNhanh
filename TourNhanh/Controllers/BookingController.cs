using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
