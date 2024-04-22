using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Controllers
{
    public class InternationalTourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
