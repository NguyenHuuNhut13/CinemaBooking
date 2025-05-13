using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Areas.Admin.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên người dùng là bắt buộc.")]
        [StringLength(255, ErrorMessage = "Tên người dùng không được vượt quá 255 ký tự.")]
        [Display(Name = "Tên Người Dùng")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
        [Display(Name = "Mật khẩu")]
        public string? PasswordHash { get; set; }

        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        [Display(Name = "Số Điện Thoại")]
        public string? PhoneNumber { get; set; }

        [StringLength(20, ErrorMessage = "Vai trò không được vượt quá 20 ký tự.")]
        [Display(Name = "Vai Trò")]
        public string? Role { get; set; }
    }
}
