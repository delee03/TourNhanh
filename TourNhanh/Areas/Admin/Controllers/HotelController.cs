using Microsoft.AspNetCore.Mvc;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HotelController : Controller
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HotelController(IHotelRepository hotelRepository, IWebHostEnvironment hostingEnvironment)
        {
            _hotelRepository = hotelRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Hotels
        public async Task<IActionResult> Index()
        {
            return View(await _hotelRepository.GetAllAsync());
        }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _hotelRepository.GetByIdAsync(id.Value);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Rating")] Hotel hotel, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                await _hotelRepository.CreateAsync(hotel);
                if (imageFile != null)
                {
                    // Handle image saving and updating MainImageUrl
                    await HandleImageSaveAndUpdate(hotel, imageFile);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _hotelRepository.GetByIdAsync(id.Value);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Rating")] Hotel hotelFromForm, IFormFile imageFile)
        {
            if (id != hotelFromForm.Id)
            {
                return NotFound();
            }
            var hotel = await _hotelRepository.GetByIdAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            hotel.Name = hotelFromForm.Name;
            hotel.Address = hotelFromForm.Address;
            hotel.Rating = hotelFromForm.Rating;

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    // Handle image saving and updating MainImageUrl
                    await HandleImageSaveAndUpdate(hotel, imageFile);
                }
                await _hotelRepository.UpdateAsync(hotel);
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _hotelRepository.GetByIdAsync(id.Value);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _hotelRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task HandleImageSaveAndUpdate(Hotel hotel, IFormFile imageFile)
        {
            // Create a new directory for the tour
            var path = Path.Combine("wwwroot", $"hotel/image/{hotel.Id}");
            Directory.CreateDirectory(path);

            if (!string.IsNullOrEmpty(hotel.ImageUrl))
            {
                var oldImagePath = ConvertUrlToFilePath(hotel.ImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Generate a random file name
            var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);

            // Save the image file to the new directory
            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Update MainImageUrl
            hotel.ImageUrl = $"/hotel/image/{hotel.Id}/{fileName}";
            await _hotelRepository.UpdateAsync(hotel);
        }

        private string ConvertUrlToFilePath(string imageUrl)
        {
            // Remove the leading slash from the URL
            var urlWithoutLeadingSlash = imageUrl.TrimStart('/');

            // Combine the URL with the root path
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, urlWithoutLeadingSlash);

            return filePath;
        }
    }
}
