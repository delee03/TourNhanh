using System.Collections.Generic;
using System.Threading.Tasks;
using TourNhanh.Models;

namespace TourNhanh.Repositories.Interfaces
{
	public interface IReviewRepository
	{
		Task<IEnumerable<Review>> GetAllAsync();
		Task<Review> GetByIdAsync(int id);
		Task<List<Review>> GetReviewsByTourId(int id); // Lấy tất cả các đánh giá của một tour
		Task AddAsync(Review review);
		Task UpdateAsync(Review review);
		Task DeleteAsync(int id);
		Task<List<Review>> GetByAuthorAsync(string author);
		Task<List<Review>> GetByRatingAsync(int rating); // Lấy tất cả các đánh giá có rating nhất định
	}
}
