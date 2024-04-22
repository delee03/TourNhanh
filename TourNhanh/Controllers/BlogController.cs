using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
