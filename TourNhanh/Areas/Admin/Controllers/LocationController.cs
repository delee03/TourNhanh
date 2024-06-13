using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Implementations;
using TourNhanh.Repositories.Interfaces;
using TourNhanh.ViewModel;

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

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            return View(await _locationRepository.GetAllAsync());
        }

        // GET: Locations/Details/5
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

        /*// GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Latitude,Longitude,Address")] Location location)
        {
            if (ModelState.IsValid)
            {
                await _locationRepository.CreateAsync(location);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }*/

        [HttpGet]
        public IActionResult Create()
        {
            return View(new LocationViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var location = new Location
                {
                    Name = model.Name,
                    Description = model.Description,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Address = string.Join("-", model.Addresses.Where(a => !string.IsNullOrEmpty(a))),
                    Desc1 = model.Descriptions.ElementAtOrDefault(0),
                    Desc2 = model.Descriptions.ElementAtOrDefault(1),
                    Desc3 = model.Descriptions.ElementAtOrDefault(2)
                };

                await _locationRepository.CreateAsync(location);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
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

            var model = new LocationViewModel
            {
                Id = location.Id,
                Name = location.Name,
                Description = location.Description,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Addresses = location.Address.Split('-').ToList(),
                Descriptions = new List<string> { location.Desc1, location.Desc2, location.Desc3 }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LocationViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var location = new Location
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Address = string.Join("-", model.Addresses.Where(a => !string.IsNullOrEmpty(a))),
                    Desc1 = model.Descriptions.ElementAtOrDefault(0),
                    Desc2 = model.Descriptions.ElementAtOrDefault(1),
                    Desc3 = model.Descriptions.ElementAtOrDefault(2)
                };

                await _locationRepository.UpdateAsync(location);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        /*// GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Locations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Latitude,Longitude,Address")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _locationRepository.UpdateAsync(location);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }*/

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _locationRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
