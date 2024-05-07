using TourNhanh.Models;

namespace TourNhanh.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int id);
        Task<List<Comment>> GetCommentsByBlogId(int blogId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(int id);
        Task<List<Blog>> GetByAuthorAsync(string author);
        Task<List<Blog>> GetByTitleKeywordAsync(string keyword);
    }
}
