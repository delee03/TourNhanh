﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITransportRepository _transportRepository;

        public TourController(ITourRepository tourRepository, ICategoryRepository categoryRepository, ITransportRepository transportRepository)
        {
            _tourRepository = tourRepository;
            _categoryRepository = categoryRepository;
            _transportRepository = transportRepository;
        }

        // GET: Tour
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

            return View(tour);
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
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Description,Price,TransportId")] Tour tour, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                await _tourRepository.CreateAsync(tour);
                // Check if an image file is provided
                if (imageFile != null)
                {
                    // Handle image saving and updating MainImageUrl
                    await HandleImageSaveAndUpdate(tour, imageFile);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tour);
        }

        private async Task HandleImageSaveAndUpdate(Tour tour, IFormFile imageFile)
        {
            // Create a new directory for the tour
            var path = Path.Combine("wwwroot", $"tour/image/{tour.Id}");
            Directory.CreateDirectory(path);

            // Generate a random file name
            var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);

            // Save the image file to the new directory
            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            // Update MainImageUrl
            tour.MainImageUrl = $"~/tour/image/{tour.Id}/{fileName}";
            await _tourRepository.UpdateAsync(tour);
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
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name", tour.CategoryId);
            ViewBag.TransportId = new SelectList(await _transportRepository.GetAllAsync(), "Id", "Name", tour.TransportId);
            return View(tour);
        }

        // POST: Tours/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Name,Description,Price,TransportId,MainImageUrl")] Tour tour)
        {
            if (id != tour.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
    }
}