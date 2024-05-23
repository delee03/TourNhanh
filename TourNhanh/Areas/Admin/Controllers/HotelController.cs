using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Implementations;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _hotelRepository;

        public HotelController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        
        public async Task<IActionResult> Index()
        {
            var hotel = await _hotelRepository.GetAllAsync();
            ViewBag.Hotels= hotel;
            return View(hotel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            return View(hotel);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Hotel hotel)
        {
            if(ModelState.IsValid)
            {
                await _hotelRepository.CreateAsync(hotel);
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        public async Task<IActionResult> Update(int id)
        {

            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            ViewBag.hotel = hotel;
            return View(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Hotel hotel)
        {
            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCategory = await _hotelRepository.GetByIdAsync(id);
                existingCategory.Name = hotel.Name;
                existingCategory.Address = hotel.Address;               
                existingCategory.Rating = hotel.Rating;
                existingCategory.ImageUrl = hotel.ImageUrl;
                await _hotelRepository.UpdateAsync(existingCategory);
                return RedirectToAction(nameof(Index));
            }

            return View(hotel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            ViewBag.hotel = hotel;
            return View(hotel);
        }
        //Process the product deletion
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _hotelRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }



    }
}
