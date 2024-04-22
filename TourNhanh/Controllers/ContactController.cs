using Microsoft.AspNetCore.Mvc;

namespace TourNhanh.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
