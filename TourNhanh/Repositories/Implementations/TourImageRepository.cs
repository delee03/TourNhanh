using Microsoft.EntityFrameworkCore;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
    public class TourImageRepository:ITourImage
    {
        private readonly AppDbContext _context;

        public TourImageRepository(AppDbContext context)
        {
            _context = context;
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
    }
}
