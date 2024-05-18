using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Controllers
{
    public class TourDetailController:Controller
    {
        private readonly ITourDetail _tourDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IHotelRepository _hotelRepository;

        public TourDetailController(ITourDetail tourDetailRepository, ILocationRepository locationRepository, IHotelRepository hotelRepository)
        {
            _tourDetailRepository = tourDetailRepository;
            _locationRepository = locationRepository;
            _hotelRepository = hotelRepository;
        }

        // GET: TourDetails
        public async Task<IActionResult> Index(int tourId)
        {
            ViewBag.TourId = tourId;
            return View(await _tourDetailRepository.GetByTourIdAsync(tourId));
        }

        // GET: TourDetails/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: TourDetails/Create
        public async Task<IActionResult> Create(int tourId)
        {
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(),"Id","Name");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name");
            var tourDetail = new TourDetail { TourId = tourId };
            ViewBag.TourId = tourId;
            return View(tourDetail);
        }

        // POST: TourDetails/Create
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

        // GET: TourDetails/Edit/5
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

        // POST: TourDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id ,[Bind("Id,TourId,LocationId,Order,StartTime,EndTime,HotelId")] TourDetail tourDetail)
        {
            if (id != tourDetail.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _tourDetailRepository.UpdateAsync(tourDetail);
                return RedirectToAction(nameof(Index),new {tourId=tourDetail.TourId });
            }
            ViewBag.LocationId = new SelectList(await _locationRepository.GetAllAsync(), "Id", "Name");
            ViewBag.HotelId = new SelectList(await _hotelRepository.GetAllAsync(), "Id", "Name");
            return View(tourDetail);
        }

        // GET: TourDetails/Delete/5
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

        // POST: TourDetails/Delete/5
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