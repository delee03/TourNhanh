using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Implementations;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocationController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public async Task<IActionResult> Index()
        {
           var location = await _locationRepository.GetAllAsync();
            ViewBag.Locations = location;
            return View(location);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _locationRepository.GetByIdAsync(id.Value);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Location location)
        {
            if (ModelState.IsValid)
            {
                await _locationRepository.CreateAsync(location);
                return RedirectToAction(nameof(Index));
            }

            return View(location);
        }
        public async Task<IActionResult> Update(int id)
        {

            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewBag.location = location;
            return View(location);
        }

        [HttpPost] 
        public async Task<IActionResult> Update(int id, Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCategory = await _locationRepository.GetByIdAsync(id);
                    existingCategory.Name = location.Name;
                    existingCategory.Description = location.Description;
                    existingCategory.Longitude = location.Longitude;
                    existingCategory.Latitude = location.Latitude;
                    existingCategory.Address = location.Address;
                await _locationRepository.UpdateAsync(existingCategory);
                return RedirectToAction(nameof(Index));
            }

                return View(location);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var location = await _locationRepository.GetByIdAsync(id);
                if (location == null)
                {
                return NotFound();
            }
            ViewBag.location = location;
            return View(location);
        }
        //Process the product deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _locationRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
