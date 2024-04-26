using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Controllers
{
    public class HotelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
