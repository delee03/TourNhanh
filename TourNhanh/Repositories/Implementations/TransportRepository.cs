using Microsoft.EntityFrameworkCore;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
    public class TransportRepository: ITransportRepository
    {
        private readonly AppDbContext _context;

        public TransportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transport>> GetAllAsync()
        {
            return await _context.Transports.ToListAsync();
        }

        public async Task<Transport?> GetByIdAsync(int id)
        {
            return await _context.Transports.FindAsync(id);
        }

        public async Task CreateAsync(Transport transport)
        {
            _context.Transports.Add(transport);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transport transport)
        {
            _context.Transports.Update(transport);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transport = await GetByIdAsync(id);
            if (transport != null)
            {
                _context.Transports.Remove(transport);
                await _context.SaveChangesAsync();
            }
        }
    }
}
