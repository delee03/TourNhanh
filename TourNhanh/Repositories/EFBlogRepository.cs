using Microsoft.EntityFrameworkCore;
using TourNhanh.Models;

namespace TourNhanh.Repositories
{
    public class EFBlogRepository: IBlogRepository
    {
        private readonly AppDbContext _context;
        public EFBlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            return await _context.Blogs.ToListAsync();
        }
        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.FindAsync(id);
        }
        public async Task AddAsync(Blog blog)
        {
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Blog blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var blogs = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blogs);
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
