using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Controllers
{
    public class TourDetailController:Controller
    {
        private readonly ITourDetail _tourDetailRepository;

        public TourDetailController(ITourDetail tourDetailRepository)
        {
            _tourDetailRepository = tourDetailRepository;
        }

        // GET: TourDetails
        public async Task<IActionResult> Index(int tourId)
        {
            return View(await _tourDetailRepository.GetByTourIdAsync(tourId));
        }

        // GET: TourDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourDetail = await _tourDetailRepository.Get(id.Value);
            if (tourDetail == null)
            {
                return NotFound();
            }

            return View(tourDetail);
        }

        // GET: TourDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TourDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TourId,LocationId,Order,StartTime,EndTime,HotelId")] TourDetail tourDetail)
        {
            if (ModelState.IsValid)
            {
                await _tourDetailRepository.AddTourDetail(tourDetail);
                return RedirectToAction(nameof(Index));
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

            var tourDetail = await _tourDetailRepository.GetTourDetail(id.Value);
            if (tourDetail == null)
            {
                return NotFound();
            }
            return View(tourDetail);
        }

        // POST: TourDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TourId,LocationId,Order,StartTime,EndTime,HotelId")] TourDetail tourDetail)
        {
            if (id != tourDetail.TourId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tourDetailRepository.UpdateTourDetail(tourDetail);
                return RedirectToAction(nameof(Index));
            }
            return View(tourDetail);
        }

        // GET: TourDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tourDetail = await _tourDetailRepository.GetTourDetail(id.Value);
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
            await _tourDetailRepository.DeleteTourDetail(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
