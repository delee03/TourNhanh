using TourNhanh.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TourNhanh.Repositories.Interfaces;

namespace TourNhanh.Repositories.Implementations
{
    public class EFLikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;

        public EFLikeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Like>> GetAllAsync()
        {
            return await _context.Likes.ToListAsync();
        }


        public async Task<Like> HasLikedAsync(string userId, int blogId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.BlogId == blogId);
        }

        public async Task AddAsync(Like like)
        {
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(string userId, int blogId)
        {
            var like = await _context.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.BlogId == blogId);

            if (like != null)
            {
                _context.Likes.Remove(like);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateAsync(Like like)
        {
            _context.Likes.Update(like);
            await _context.SaveChangesAsync();
        }
    }
}
