using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Areas.Admin.ViewModels
{
    public class CinemaViewModel
    {
        [Required(ErrorMessage = "Tên rạp chiếu phim là bắt buộc.")]
        [StringLength(255, ErrorMessage = "Tên rạp chiếu phim không được vượt quá 255 ký tự.")]
        [Display(Name = "Tên Rạp")]
        public string Name { get; set; } = null!;

        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự.")]
        [Display(Name = "Địa Chỉ")]
        public string? Location { get; set; }

        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự.")]
        [Display(Name = "Số Điện Thoại")]
        public string? PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự.")]
        [Display(Name = "Thành Phố")]
        public string? City { get; set; }
    }
}
