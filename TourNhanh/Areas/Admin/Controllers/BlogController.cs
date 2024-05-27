using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
