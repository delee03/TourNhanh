using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            // Get a list of all customers
            var User = await _userManager.GetUsersInRoleAsync("Customer");
            ViewBag.Users = User;
            return View();
        }
        // GET: Customer/Details/5
        public async Task<IActionResult> Detail(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Customer = customer;

            return View();
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var customer = await _userManager.FindByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            // Delete the customer
            var result = await _userManager.DeleteAsync(customer);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(customer);
        }

    }
}
