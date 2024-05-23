function addComment(blogId, content) {
    $.ajax({
        type: "POST",
        url: "/Blog/AddComment",
        data: { blogId: blogId, content: content },
        success: function (response) {
            // Xử lý kết quả trả về (nếu cần)
            alert("Bình luận đã được thêm thành công!");
            // Cập nhật giao diện nếu cần
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi (nếu có)
            alert("Đã xảy ra lỗi: " + error);
        }
    });
}
