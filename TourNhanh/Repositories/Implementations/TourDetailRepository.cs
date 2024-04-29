using Microsoft.EntityFrameworkCore;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
    public class TourDetailRepository : ITourDetail
    {
        private readonly AppDbContext _context;

        public TourDetailRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourDetail>> GetAllAsync()
        {
            return await _context.TourDetails.ToListAsync();
        }

        public async Task<TourDetail?> GetByIdAsync(int id)
        {
            return await _context.TourDetails.FindAsync(id);
        }

        public async Task<IEnumerable<TourDetail>> GetByTourIdAsync(int tourId)
        {
            return await _context.TourDetails.Where(td => td.TourId == tourId).ToListAsync();
        }

        public async Task CreateAsync(TourDetail tourDetail)
        {
            _context.TourDetails.Add(tourDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TourDetail tourDetail)
        {
            _context.TourDetails.Update(tourDetail);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tourDetail = await _context.TourDetails.FindAsync(id);
            if (tourDetail != null)
            {
                _context.TourDetails.Remove(tourDetail);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByTourIdAsync(int tourId)
        {
            var tourDetails = _context.TourDetails.Where(td => td.TourId == tourId);
            _context.TourDetails.RemoveRange(tourDetails);
            await _context.SaveChangesAsync();
        }
    }
}
