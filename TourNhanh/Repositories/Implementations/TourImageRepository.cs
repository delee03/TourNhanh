using Microsoft.EntityFrameworkCore;
using TourNhanh;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
    public class TourImageRepository : ITourImage
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TourImageRepository(AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IEnumerable<TourImage>> GetAllAsync()
        {
            return await _context.TourImages.ToListAsync();
        }

        public async Task<TourImage?> GetByIdAsync(int id)
        {
            return await _context.TourImages.FindAsync(id);
        }

        public async Task CreateAsync(TourImage tourImage)
        {
            _context.TourImages.Add(tourImage);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TourImage tourImage)
        {
            _context.TourImages.Update(tourImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tourImage = await GetByIdAsync(id);
            if (tourImage != null)
            {
                _context.TourImages.Remove(tourImage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TourImage>> GetByTourIdAsync(int tourId)
        {
            return await _context.TourImages.Where(ti => ti.TourId == tourId).ToListAsync();
        }

        public async Task DeleteByTourIdAsync(int tourId)
        {
            var tourImages = await GetByTourIdAsync(tourId);
            if (tourImages != null)
            {
                foreach (var tourImage in tourImages)
                {
                    // Convert the URL to a file path
                    if (tourImage.ImageUrl != null)
                    {
                        var filePath = ConvertUrlToFilePath(tourImage.ImageUrl);

                        // Delete the image file
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }

                }
                _context.TourImages.RemoveRange(tourImages);
                await _context.SaveChangesAsync();
            }
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
