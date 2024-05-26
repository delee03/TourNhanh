using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize(Roles = "Admin")]*/
    public class TourDetailController : Controller
    {
        private readonly ITourDetail _tourDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ITourRepository _tourRepository;
        private readonly ITourImage _tourImageRepository;

        public TourDetailController(ITourDetail tourDetailRepository, ILocationRepository locationRepository, IHotelRepository hotelRepository, ITourRepository tourRepository, ITourImage tourImageRepository)
        {
            _tourDetailRepository = tourDetailRepository;
            _locationRepository = locationRepository;
            _hotelRepository = hotelRepository;
            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
        }

        // GET: Admin/TourDetails
        public async Task<IActionResult> Index(int tourId)
        {
            var details = await _tourDetailRepository.GetByTourIdAsync(tourId);
            ViewBag.TourId = tourId;
            List<string> imageURL = new List<string>();
            var tourimage = await _tourImageRepository.GetByTourIdAsync(tourId);
            foreach (var img in tourimage)
            {
                imageURL.Add(img.ImageUrl);
            }
            ViewBag.TourImage = imageURL;
            var tour = await _tourRepository.GetByIdAsync(tourId);
            ViewBag.Tour = tour;
            return View(details);
        }

        // GET: Admin/TourDetails/Create
        public async Task<IActionResult> Create(int tourId)
        {
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(), "Id", "Name", "Address");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name", "Address");
            var tourDetail = new TourDetail { TourId = tourId };
            ViewBag.TourId = tourId;
            return View(tourDetail);
        }

        // POST: Admin/TourDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,LocationId,Order,StartTime,EndTime,HotelId")] TourDetail tourDetail)
        {
            if (ModelState.IsValid)
            {
                await _tourDetailRepository.CreateAsync(tourDetail);
                return RedirectToAction(nameof(Index), new { tourId = tourDetail.TourId });
            }
            return View(tourDetail);
        }

        // GET: Admin/TourDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourDetail = await _tourDetailRepository.GetByIdAsync(id.Value);
            if (tourDetail == null)
            {
                return NotFound();
            }
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(), "Id", "Name");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name");
            return View(tourDetail);
        }

        // POST: Admin/TourDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TourId,LocationId,Order,StartTime,EndTime,HotelId")] TourDetail tourDetail)
        {
            if (id != tourDetail.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _tourDetailRepository.UpdateAsync(tourDetail);
                return RedirectToAction(nameof(Index), new { tourId = tourDetail.TourId });
            }
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(), "Id", "Name");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name");
            return View(tourDetail);
        }

        // GET: Admin/TourDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourDetail = await _tourDetailRepository.GetByIdAsync(id.Value);
            if (tourDetail == null)
            {
                return NotFound();
            }

            return View(tourDetail);
        }

        // POST: Admin/TourDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tourDetail = await _tourDetailRepository.GetByIdAsync(id);
            var tourId = tourDetail?.TourId;
            await _tourDetailRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { tourId = tourId });
        }
    }
}
