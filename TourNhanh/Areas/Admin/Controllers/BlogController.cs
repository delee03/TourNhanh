using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
