using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Controllers
{
    public class NationalTourController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
