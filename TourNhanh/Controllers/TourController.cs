using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Controllers
{
    public class TourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
