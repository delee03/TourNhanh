using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Linq;
using TourNhanh.Models;
using TourNhanh.Repositories;

namespace TourNhanh.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILikeRepository _likeRepository;

        public BlogController(IBlogRepository blogRepository, ICommentRepository commentRepository, UserManager<AppUser> userManager, ILikeRepository likeRepository)
        {

            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _userManager = userManager;
            _likeRepository = likeRepository;
        }
        public async Task<IActionResult> Index(string title)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                ViewBag.UserId = currentUser.Id;
            }
            var blogs = await _blogRepository.GetAllAsync();
            foreach (var item in blogs)
            {
                var b = await _blogRepository.GetByIdAsync(item.Id);
                var comments = await _commentRepository.GetCommentsByBlogId(item.Id);
                b.Comments = comments;
            }
            if (!string.IsNullOrEmpty(title))
            {
                blogs = blogs.Where(x => x.Title.Contains(title));
            }
            return View(blogs);
        }

        public async Task<IActionResult> Add()
        {

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(Blog blog, IFormFile imageUrl)
        {

            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    blog.ImageUrl = await SaveImage(imageUrl);
                }
                var currentUser = await _userManager.GetUserAsync(User);
                // Thay thế Author bằng tên của người dùng hiện tại
                blog.Author = currentUser.FullName; // Hoặc bạn có thể sử dụng currentUser.UserName hoặc bất kỳ thuộc tính nào khác của người dùng

                blog.CreatedAt = DateTime.UtcNow;
                blog.UpdatedAt = DateTime.UtcNow;
                await _blogRepository.AddAsync(blog);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/img", image.FileName);
            //thay doi duong dan theo cau hinh cua bn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/img/" + image.FileName;
            //tra ve duong dan tuong doi
        }
        // Hàm lưu ảnh
        /* private async Task<string> SaveImage(IFormFile image)
         {
             var savePath = Path.Combine("wwwroot/img", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
             using (var fileStream = new FileStream(savePath, FileMode.Create))
             {
                 await image.CopyToAsync(fileStream);
             }
             return "/images/" + image.FileName; // Trả về đường dẫn tương đối
         }


 */
        public async Task<IActionResult> Display(int id)
        {
            var blogs = await _blogRepository.GetByIdAsync(id);
            var comments = await _commentRepository.GetCommentsByBlogId(id);
            blogs.Comments = comments;
            if (blogs == null)
            {
                return NotFound();
            }
            return View(blogs);
        }

        public async Task<IActionResult> Update(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Blog blog, IFormFile imageUrl)
        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl
            if (id != blog.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {


                var existingBlog = await _blogRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync


                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (imageUrl == null)
                {
                    blog.ImageUrl = existingBlog.ImageUrl;
                }
                else
                {
                    // Lưu hình ảnh mới
                    blog.ImageUrl = await SaveImage(imageUrl);
                }
                existingBlog.Title = blog.Title;
                existingBlog.Author = blog.Author;
                existingBlog.ImageUrl = blog.ImageUrl;
                existingBlog.Content = blog.Content;
                existingBlog.UpdatedAt = DateTime.UtcNow;


                await _blogRepository.UpdateAsync(existingBlog);

                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }


        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int id, string content)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = content,
                BlogId = id,
                Author = currentUser.FullName,
                CreatedAt = DateTime.UtcNow // Assuming you have this property
            };
            await _commentRepository.AddAsync(comment);

            // Return additional information about the comment
            return Json(new { success = true, id = id, author = currentUser.FullName });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            await _commentRepository.DeleteAsync(id);

            // Chuyển hướng đến trang hiển thị bài viết sau khi xóa bình luận
            return RedirectToAction("Display", "Blog", new { Id = comment.BlogId });
        }



        [HttpPost]
        public async Task<IActionResult> EditComment(int id, string content)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            // Cập nhật nội dung của bình luận
            comment.Content = content;
            await _commentRepository.UpdateAsync(comment);

            // Chuyển hướng đến trang hiển thị bài viết sau khi chỉnh sửa bình luận
            return RedirectToAction("Display", "Blog", new { Id = comment.BlogId });
        }

        /*
        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
          
            // Kiểm tra xem người dùng đã like bài viết này trước đó chưa
            var hasLiked = HttpContext.Request.Cookies["LikedPost_" + id];
            var blog = await _blogRepository.GetByIdAsync(id);

            if (hasLiked != null)
            {
                // Người dùng đã like bài viết này trước đó, do đó ta sẽ giảm số lượng like và xóa cookie
                if (blog != null && blog.Likes > 0)
                {
                    blog.Likes--;
                    await _blogRepository.UpdateAsync(blog);
                }

                // Xóa cookie để đánh dấu rằng người dùng đã unlike bài viết này
                HttpContext.Response.Cookies.Delete("LikedPost_" + id);

                return Ok("Unlike thành công.");
            }

            // Ngược lại, người dùng chưa like bài viết này, do đó ta sẽ tăng số lượng like và lưu cookie
            if (blog != null)
            {
                blog.Likes++;
                await _blogRepository.UpdateAsync(blog);
            }

            // Lưu cookie để đánh dấu rằng người dùng đã like bài viết này
            HttpContext.Response.Cookies.Append("LikedPost_" + id, "true", new CookieOptions
            {
                // Thiết lập thời gian sống của cookie tùy ý
                // Ví dụ: TimeSpan.FromDays(30) sẽ làm cookie tồn tại trong 30 ngày
                Expires = DateTimeOffset.Now.Add(TimeSpan.FromDays(30)),
                HttpOnly = true
            });

            return Ok("Like thành công.");
        }
        
            var userId = User.Identity.Name;
            var hasLiked = await _likeRepository.HasLikedAsync(userId, id);
            var blog = await _blogRepository.GetByIdAsync(id);

            if (hasLiked)
            {
                if (blog != null && blog.Likes > 0)
                {
                    blog.Likes--;
                    await _blogRepository.UpdateAsync(blog);
                }

                await _likeRepository.RemoveAsync(userId, id);

                var likeCount = blog.Likes;
                var isLiked = false;

                return Json(new { likeCount, isLiked });
            }

            if (blog != null)
            {
                blog.Likes++;
                await _blogRepository.UpdateAsync(blog);
                await _likeRepository.AddAsync(new Like { UserId = userId, BlogId = id });

                var likeCount = blog.Likes;
                var isLiked = true;

                return Json(new { likeCount, isLiked });
            }

            return Json(new { error = "Failed to like/unlike post." });
         
         
        

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            bool alreadyLiked = await _likeRepository.HasLikedAsync(currentUser.Id, id);
            if (!alreadyLiked)
            {
                var like = new Like
                {
                    UserId = currentUser.Id,
                    BlogId = id,
                    Liked = true
                };

                await _likeRepository.AddAsync(like);

                // Tăng số lượng like của bài viết
                var blog = await _blogRepository.GetByIdAsync(id);
                if (blog != null)
                {
                    blog.Likes++;
                    await _blogRepository.UpdateAsync(blog);
                }
            }
            else
            {
                await _likeRepository.RemoveAsync(currentUser.Id, id);
                var blog = await _blogRepository.GetByIdAsync(id);
                if (blog != null)
                {
                    blog.Likes--;
                    await _blogRepository.UpdateAsync(blog);
                }
            }
            var response = new { Liked = !alreadyLiked };
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckLikedStatus(int id)
        {
            // Lấy UserID của người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                // Trả về dữ liệu JSON với trạng thái liked là false nếu người dùng không được xác định
                return Json(new { Liked = false });
            }

            // Kiểm tra xem người dùng đã like bài viết có ID là 'id' chưa
            bool liked = await _likeRepository.HasLikedAsync(currentUser.Id, id);

            // Trả về dữ liệu JSON với trạng thái liked
            return Json(new { Liked = liked });
        }
        */
        [HttpPost]
        public async Task<IActionResult> LikePost(int blogId, string userId)
        {
            var existingLike = await _likeRepository.HasLikedAsync(userId, blogId);

            if (existingLike != null)
            {
                await _likeRepository.RemoveAsync(userId, blogId);

                var blog = await _blogRepository.GetByIdAsync(blogId);
                blog.Likes--;
                await _blogRepository.UpdateAsync(blog);

                return Ok(blog.Likes); // Trả về số lượng like mới sau khi unlike
            }
            else
            {
                var newLike = new Like
                {
                    BlogId = blogId,
                    UserId = userId,
                    Liked = true
                };
                await _likeRepository.AddAsync(newLike);

                var blog = await _blogRepository.GetByIdAsync(blogId);
                blog.Likes++;
                await _blogRepository.UpdateAsync(blog);
                return Ok(blog.Likes); // Trả về số lượng like mới sau khi like
            }
        }

        [HttpGet]
        public async Task<bool> CheckInitialLikeStatus(int blogId, string userId)
        {
            var existingLike = await _likeRepository.HasLikedAsync(userId, blogId);
            return existingLike != null && existingLike.Liked; // Trả về true nếu đã like, ngược lại trả về false
        }

        public async Task<bool> CheckWord(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            List<string> bannedWords = new List<string>
            {
                    "đánh", "giết", "đâm", "cắt", "tấn công",
                    "sex", "porn", "nude", "threesome", "kinky",
                    "chó", "đen", "gái", "người nước ngoài",
                    "child porn", "underage", "teen sex",
                    "stalk", "harass", "bully",
                    "drugs", "alcohol", "smoking",
                     "fake account", "impersonation"
            };
            foreach(var word in bannedWords)
            {
                if (blog.Content.Contains(word))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
