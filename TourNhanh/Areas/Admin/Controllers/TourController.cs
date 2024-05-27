using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using TourNhanh.ViewModel;
using Microsoft.Extensions.Hosting;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace TourNhanh.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class TourController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITourDetail _tourDetailRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly ITourImage _tourImageRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TourController(ITourRepository tourRepository, ICategoryRepository categoryRepository, ITransportRepository transportRepository, ITourImage tourImageRepository, IWebHostEnvironment hostingEnvironment, ITourDetail tourDetail)
        {
            _tourRepository = tourRepository;
            _categoryRepository = categoryRepository;
            _transportRepository = transportRepository;
            _tourImageRepository = tourImageRepository;
            _hostingEnvironment = hostingEnvironment;
            _tourDetailRepository = tourDetail;
        }

        // GET: Admin/Tour
        public async Task<IActionResult> Index()
        {
            return View(await _tourRepository.GetAllAsync());
        }

        // GET: Tour/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _tourRepository.GetByIdAsync(id.Value);
            if (tour == null)
            {
                return NotFound();
            }
            var tourImages = await _tourImageRepository.GetByTourIdAsync(tour.Id);
            ViewBag.TourImages = tourImages.Select(ti => ti.ImageUrl).ToList();
            return View(tour);
        }

        // GET: Admin/Tour/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.TransportId = new SelectList(await _transportRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Admin/Tour/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Description,maxParticipants,RemainingSlots,Price,TransportId")] Tour tour, IFormFile? imageFile, List<IFormFile> additionalImages)
        {
            tour.RemainingSlots = tour.maxParticipants;
            if (ModelState.IsValid)
            {
                await _tourRepository.CreateAsync(tour);
                if (imageFile != null)
                {
                    await HandleImageSaveAndUpdate(tour, imageFile);
                }
                foreach (var additionalImage in additionalImages)
                {
                    var tourImage = new TourImage
                    {
                        TourId = tour.Id,
                        ImageUrl = await SaveImageAndGetUrl(tour.Id, additionalImage)
                    };
                    await _tourImageRepository.CreateAsync(tourImage);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tour);
        }

        // GET: Admin/Tour/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _tourRepository.GetByIdAsync(id.Value);
            if (tour == null)
            {
                return NotFound();
            }
            var tourImages = await _tourImageRepository.GetByTourIdAsync(tour.Id);
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", tour.CategoryId);
            ViewBag.TransportId = new SelectList(await _transportRepository.GetAllAsync(), "Id", "Name", tour.TransportId);
            ViewBag.TourImages = tourImages.Select(ti => ti.ImageUrl).ToList();
            return View(tour);
        }

        // POST: Admin/Tour/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Name,Description,maxParticipants,RemainingSlots,Price,TransportId")] Tour tourFromForm, IFormFile? imageFile, List<IFormFile> additionalImages)
        {
            tourFromForm.RemainingSlots = tourFromForm.maxParticipants;
            if (id != tourFromForm.Id)
            {
                return NotFound();
            }

            var tour = await _tourRepository.GetByIdAsync(tourFromForm.Id);
            if (tour == null)
            {
                return NotFound();
            }

            tour.CategoryId = tourFromForm.CategoryId;
            tour.Name = tourFromForm.Name;
            tour.Description = tourFromForm.Description;
            tour.Price = tourFromForm.Price;
            tour.TransportId = tourFromForm.TransportId;
            tour.RemainingSlots = tourFromForm.maxParticipants - (tour.maxParticipants - tour.RemainingSlots);
            tour.maxParticipants = tourFromForm.maxParticipants;

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    await HandleImageSaveAndUpdate(tour, imageFile);
                }
                if (additionalImages != null && additionalImages.Count > 0)
                {
                    await _tourImageRepository.DeleteByTourIdAsync(tour.Id);
                    foreach (var additionalImage in additionalImages)
                    {
                        var tourImage = new TourImage
                        {
                            TourId = tour.Id,
                            ImageUrl = await SaveImageAndGetUrl(tour.Id, additionalImage)
                        };
                        await _tourImageRepository.CreateAsync(tourImage);
                    }
                }
                await _tourRepository.UpdateAsync(tour);
                return RedirectToAction(nameof(Index));
            }
            return View(tour);
        }



        // GET: Admin/Tour/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tour = await _tourRepository.GetByIdAsync(id.Value);
            if (tour == null)
            {
                return NotFound();
            }
            return View(tour);
        }

        // POST: Admin/Tour/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tourRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task HandleImageSaveAndUpdate(Tour tour, IFormFile imageFile)
        {
            var path = Path.Combine("wwwroot", $"tour/image/{tour.Id}");
            Directory.CreateDirectory(path);

            if (!string.IsNullOrEmpty(tour.MainImageUrl))
            {
                var oldImagePath = ConvertUrlToFilePath(tour.MainImageUrl);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            tour.MainImageUrl = $"/tour/image/{tour.Id}/{fileName}";
            await _tourRepository.UpdateAsync(tour);
        }

        private async Task<string> SaveImageAndGetUrl(int tourId, IFormFile imageFile)
        {
            var path = Path.Combine("wwwroot", $"tour/image/{tourId}");
            Directory.CreateDirectory(path);

            var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/tour/image/{tourId}/{fileName}";
        }

        private string ConvertUrlToFilePath(string imageUrl)
        {
            var urlWithoutLeadingSlash = imageUrl.TrimStart('/');
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, urlWithoutLeadingSlash);
            return filePath;
        }
    }
}
