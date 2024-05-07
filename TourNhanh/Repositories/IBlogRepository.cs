using TourNhanh.Models;

namespace TourNhanh.Repositories
{
    public interface IBlogRepository
    {
       
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog> GetByIdAsync(int id);
        Task AddAsync(Blog blog);
        Task UpdateAsync(Blog blog);
        Task DeleteAsync(int id);
        Task<List<Blog>> GetByAuthorAsync(string author);
        Task<List<Blog>> GetByTitleKeywordAsync(string keyword);

    }
}
