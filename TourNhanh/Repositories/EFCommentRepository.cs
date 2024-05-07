using Microsoft.EntityFrameworkCore;
using TourNhanh.Models;

namespace TourNhanh.Repositories
{
    public class EFCommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        public EFCommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.comments.ToListAsync();
        }
        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _context.comments.FindAsync(id);
        }
        public async Task<List<Comment>> GetCommentsByBlogId(int blogId)
        {
            return await _context.comments
                .Where(c => c.BlogId == blogId)
                .ToListAsync();
        }
        public async Task AddAsync(Comment comment)
        {
            _context.comments.Add(comment);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Comment comment)
        {
            _context.comments.Update(comment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var comment = await _context.comments.FindAsync(id);
            _context.comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Blog>> GetByAuthorAsync(string author)
        {
            return await _context.Blogs.Where(b => b.Author == author).ToListAsync();
        }

        public async Task<List<Blog>> GetByTitleKeywordAsync(string keyword)
        {
            return await _context.Blogs.Where(b => b.Title.Contains(keyword)).ToListAsync();
        }
    }
}
