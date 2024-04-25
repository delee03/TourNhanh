using Microsoft.EntityFrameworkCore;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
    public class TourRepository : ITourRepository
    {
        private readonly AppDbContext _context;

        public TourRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllAsync()
        {
            return await _context.Tours.ToListAsync();
        }

        public async Task<Tour> GetByIdAsync(int id)
        {
            return await _context.Tours.FindAsync(id);
        }

        public async Task CreateAsync(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tour tour)
        {
            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
        }
    }
}
