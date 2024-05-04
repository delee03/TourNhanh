using Microsoft.AspNetCore.Mvc;
using TourNhanh.DataQuery;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        QueryData dataquery = new QueryData();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(object obj)
        {
            //string username = Convert.ToString(HttpContext.Request.Form["email"]) ?? string.Empty;
            string password = Convert.ToString(HttpContext.Request.Form["password"]) ?? string.Empty;
            if (dataquery.loginAdmin(password))
            {
                HttpContext.Session.SetString("admin", password);
                return RedirectToAction("Index", "DashBoard");
            }
            return View();
        }
    }
}
