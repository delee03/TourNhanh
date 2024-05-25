using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using TourNhanh.ViewModel;
using Microsoft.Extensions.Hosting;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using TourNhanh.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace TourNhanh.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITourDetail _tourDetailRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly ITourImage _tourImageRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly UserManager<AppUser> _userManager;
        private readonly IReviewRepository _reviewRepository;

		public TourController(ITourRepository tourRepository, ICategoryRepository categoryRepository, ITransportRepository transportRepository, ITourImage tourImageRepository,IWebHostEnvironment hostingEnvironment, ITourDetail tourDetail, UserManager<AppUser> userManager, IReviewRepository reviewRepository)
        {
            _tourRepository = tourRepository;
            _categoryRepository = categoryRepository;
            _transportRepository = transportRepository;
            _tourImageRepository = tourImageRepository;
            _hostingEnvironment = hostingEnvironment;
            _tourDetailRepository = tourDetail;
			_userManager = userManager;
            _reviewRepository = reviewRepository;
		}

        // GET: Tour
        public async Task<IActionResult> Index(string name, decimal? Priceto, decimal? Pricefrom, int? CategoryId)
        {
			var tours = await _tourRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories;
            if (!string.IsNullOrEmpty(name))
			{
				if (Priceto != null && Pricefrom != null)
				{
					if (CategoryId > 0)
					{
                        tours = tours.Where(x => x.CategoryId == CategoryId &&
                          x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 &&
                          x.Price >= Priceto &&
                          x.Price <= Pricefrom);

                    }
                    else
					{
						tours = tours.Where(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 && x.Price >= Priceto && x.Price <= Pricefrom);

					}

				}
				else
				{
					if(CategoryId > 0)
                    {
                        tours = tours.Where(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 && x.CategoryId == CategoryId);
                    }
                    else
                    {
                        tours = tours.Where(x => x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                    }

				}
			}
			else
			{
				if(CategoryId > 0)
                {
                    if(Priceto != null && Pricefrom != null)
                    {
                        tours = tours.Where(x => x.CategoryId == CategoryId && x.Price >= Priceto && x.Price <= Pricefrom);
                    }
                    else
                    {
                        tours = tours.Where(x => x.CategoryId == CategoryId);
                    }
                }
                else
                {
                    if (Priceto != null && Pricefrom != null)
                    {
                        tours = tours.Where(x => x.Price >= Priceto && x.Price <= Pricefrom);
                    }
                }
            }
			foreach (var item in tours)
			{
				var t = await _tourRepository.GetByIdAsync(item.Id);
				var review = await _reviewRepository.GetReviewsByTourId(t.Id);
				t.Reviews = review;
			}
			return View(tours);
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
            var tours = await _tourRepository.GetAllAsync();
            foreach(var item in tours)
            {
                var t = await _tourRepository.GetByIdAsync(item.Id);
                var review = await _reviewRepository.GetReviewsByTourId(t.Id);
                t.Reviews = review;
            }
			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser != null)
			{
				ViewBag.CurrentUserFullName = currentUser.FullName;
				ViewBag.CurrentUserEmail = currentUser.Email;
			}
			var tourImages = await _tourImageRepository.GetByTourIdAsync(tour.Id);
            ViewBag.TourImages = tourImages.Select(ti => ti.ImageUrl).ToList();
            return View(tour);
        }

        //ViewDirectory 
        public async Task<IActionResult> LichTrinh(int? id, int ?idDetail)
        {
			if (id == null && idDetail == null)
			{
				return NotFound();
			}
			var tour = await _tourRepository.GetByIdAsync(id.Value);
            var tourDetail = await _tourDetailRepository.GetByIdAsync(idDetail.Value);
			var viewmodel = new TourDetail_LichTrinh()
            {
                Id = tour.Id,
                Name = tour.Name,
                Description = tour.Description,
                Category = tour.Category,
                Transport = tour.Transport,
                Price = tour.Price,
                MainImageUrl = tour.MainImageUrl,
                TourDetailId = tourDetail.TourId,
                Order = tourDetail.Order,
                Location = tourDetail.Location,
                StartTime= tourDetail.StartTime,
                EndTime= tourDetail.EndTime,
                Hotel= tourDetail.Hotel,
            };
            ViewBag.ViewModel = viewmodel;
            return View(viewmodel);
        }

            // GET: Tour/Create
            public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.TransportId = new SelectList(await _transportRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Tour/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Description,maxParticipants,RemainingSlots,Price,TransportId")] Tour tour, IFormFile? imageFile, List<IFormFile> additionalImages)
        {
            tour.RemainingSlots = tour.maxParticipants;
            if (ModelState.IsValid)
            {
                await _tourRepository.CreateAsync(tour);
                // Check if an image file is provided
                if (imageFile != null)
                {
                    // Handle image saving and updating MainImageUrl
                    await HandleImageSaveAndUpdate(tour, imageFile);
                }
                // Handle additional images
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

        

        // GET: Tour/Edit/5
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

        // POST: Tours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Name,Description,maxParticipants,Price,TransportId")] Tour tourFromForm, IFormFile? imageFile, List<IFormFile> additionalImages)
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

            // Update the tour fields based on the form data
            tour.CategoryId = tourFromForm.CategoryId;
            tour.Name = tourFromForm.Name;
            tour.Description = tourFromForm.Description;
            tour.Price = tourFromForm.Price;
            tour.TransportId = tourFromForm.TransportId;
            tour.RemainingSlots = tourFromForm.maxParticipants - (tour.maxParticipants- tour.RemainingSlots);
            tour.maxParticipants = tourFromForm.maxParticipants;
            
            if (ModelState.IsValid)
            {
                // Check if an image file is provided
                if (imageFile != null)
                {
                    // Handle image saving and updating MainImageUrl
                    await HandleImageSaveAndUpdate(tour, imageFile);
                }
                // Handle additional images
                if (additionalImages != null && additionalImages.Count > 0)
                {
                    // Delete old additional images
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

        // GET: Tours/Delete/5
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

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tourRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task HandleImageSaveAndUpdate(Tour tour, IFormFile imageFile)
        {
            // Create a new directory for the tour
            var path = Path.Combine("wwwroot", $"tour/image/{tour.Id}");
            Directory.CreateDirectory(path);

            // If a main image already exists, delete it
            if (!string.IsNullOrEmpty(tour.MainImageUrl))
            {
                var oldImagePath = ConvertUrlToFilePath(tour.MainImageUrl);
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
            tour.MainImageUrl = $"/tour/image/{tour.Id}/{fileName}";
            await _tourRepository.UpdateAsync(tour);
        }

        private async Task<string> SaveImageAndGetUrl(int tourId, IFormFile imageFile)
        {
            // Create a new directory for the tour
            var path = Path.Combine("wwwroot", $"tour/image/{tourId}");
            Directory.CreateDirectory(path);

            // Generate a random file name
            var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);

            // Save the image file to the new directory
            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Return the image URL
            return $"/tour/image/{tourId}/{fileName}";
        }
        private string ConvertUrlToFilePath(string imageUrl)
        {
            // Remove the leading slash from the URL
            var urlWithoutLeadingSlash = imageUrl.TrimStart('/');

            // Combine the URL with the root path
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, urlWithoutLeadingSlash);

            return filePath;
        }

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddReview(int id, string content, int rating)
		{
			var currentUser = await _userManager.GetUserAsync(User);
			if (currentUser == null)
			{
				return Unauthorized();
			}

			var tour = await _tourRepository.GetByIdAsync(id);
			if (tour == null)
			{
				return NotFound();
			}

			var review = new Review
			{
				TourId = tour.Id,
				Author = currentUser.FullName,
				Email = currentUser.Email,
				Content = content,
				Rating = rating,
				CreatedAt = DateTime.UtcNow
			};

			await _reviewRepository.AddAsync(review);

			return Json(new
			{
				success = true,
				id = review.TourId,
				author = currentUser.FullName,
				content = review.Content,
				createdAt = review.CreatedAt.ToString("dd/MM/yyyy HH:mm"),
				rating = review.Rating
			});
		}
		

		[HttpPost]
		public async Task<IActionResult> DeleteReview(int id)
		{
            var review = await _reviewRepository.GetByIdAsync(id);
			if (review == null)
			{
				return NotFound();
			}

            await _reviewRepository.DeleteAsync(id);

			// Chuyển hướng đến trang hiển thị bài viết sau khi xóa bình luận
			return RedirectToAction("Display", "Tour", new { Id = review.TourId });
		}



		[HttpPost]
		public async Task<IActionResult> EditReview(int id, string content)
		{
			var review = await _reviewRepository.GetByIdAsync(id);
			if (review == null)
			{
				return NotFound();
			}

			// Cập nhật nội dung của bình luận
			review.Content = content;
            await _reviewRepository.UpdateAsync(review);

			// Chuyển hướng đến trang hiển thị bài viết sau khi chỉnh sửa bình luận
			return RedirectToAction("Display", "Review", new { Id = review.TourId });
		}

	}
}
