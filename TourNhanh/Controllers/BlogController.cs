using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Linq;
using TourNhanh.Models;
using TourNhanh.Repositories.Interfaces;

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
                blogs = blogs.Where(x => x.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return View(blogs);
        }
        [Authorize]
        public IActionResult Add()
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
                    blog.ImageUrl = await SaveImage(imageUrl);
                }
                var currentUser = await _userManager.GetUserAsync(User);
                blog.Author = currentUser.FullName;
                blog.CreatedAt = DateTime.Now;
                blog.UpdatedAt = DateTime.Now;
                await _blogRepository.AddAsync(blog);
                return Json(new { success = true });
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
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
            ModelState.Remove("ImageUrl");
            if (id != blog.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {


                var existingBlog = await _blogRepository.GetByIdAsync(id);



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
                CreatedAt = DateTime.UtcNow,
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

        public async Task<IActionResult> Change(int id)
        {
            var blogs = await _blogRepository.GetByIdAsync(id);
            if (blogs == null)
            {
                return NotFound();
            }
            return View(blogs);
        }

        public async Task<IActionResult> Accept(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Chỉ cần cập nhật trạng thái của blog thành 1 (hoặc giá trị tương ứng với trạng thái đã chấp nhận)
            blog.State = 1; // Assuming 1 represents accepted state
            await _blogRepository.UpdateAsync(blog);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Reject(int id)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Xóa blog khỏi cơ sở dữ liệu
            await _blogRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
