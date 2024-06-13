using Microsoft.EntityFrameworkCore;
using TourNhanh;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
    public class HotelRepository:IHotelRepository
    {
        private readonly AppDbContext _context;

        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Hotels.ToListAsync();
        }

        public async Task<Hotel?> GetByIdAsync(int id)
        {
            return await _context.Hotels.FindAsync(id);
        }

        public async Task CreateAsync(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hotel = await GetByIdAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                await _context.SaveChangesAsync();
            }
        }
    }
}
