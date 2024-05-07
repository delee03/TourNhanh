using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TourNhanh.Migrations;
using TourNhanh.Models;
using TourNhanh.Repositories;

namespace TourNhanh.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;

        public BlogController(IBlogRepository blogRepository, ICommentRepository commentRepository)
        {

            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _blogRepository.GetAllAsync();
            foreach(var item in blogs)
            {
                var b = await _blogRepository.GetByIdAsync(item.Id);
                var comments = await _commentRepository.GetCommentsByBlogId(item.Id);
                b.Comments = comments;
            }
            return View(blogs);
        }

        public async Task<IActionResult> Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Blog blog, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện tham khảo bài 02 hàm SaveImage
                    blog.ImageUrl = await SaveImage(imageUrl);
                }
                blog.CreatedAt = DateTime.UtcNow;
                blog.UpdatedAt = DateTime.UtcNow;
                await _blogRepository.AddAsync(blog);
                return RedirectToAction(nameof(Index));
            }
            return View(blog);

            /*  catch (Exception ex)
              {
                  ModelState.AddModelError("", $"Lỗi khi thêm bài viết: {ex.Message}");
                  return View(blog);
              }*/
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
        public async Task<IActionResult> AddComment(int id, string content)
        {
            var blog = await _blogRepository.GetByIdAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            var comment = new Comment { Content = content, BlogId = id };
            await _commentRepository.AddAsync(comment);

            // Trả về thông tin về comment mới dưới dạng JSON
            return Json(new { success = true, comment = comment });
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
        public async Task<IActionResult> Like(int id)
        {
            // Tìm bài viết có id tương ứng trong cơ sở dữ liệu
            var post = await _blogRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Tăng số lượng like của bài viết và lưu vào cơ sở dữ liệu
            post.Likes++;
            await _blogRepository.UpdateAsync(post);

            return Ok();
        }



    }
}
