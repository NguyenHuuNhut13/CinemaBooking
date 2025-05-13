using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Areas.Admin.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc.")]
        [Display(Name = "Tiêu Đề")]
        [StringLength(255, ErrorMessage = "Tiêu đề không được vượt quá 255 ký tự.")]
        public string Title { get; set; } = null!;

        [Display(Name = "Thể Loại")]
        [StringLength(100, ErrorMessage = "Thể loại không được vượt quá 100 ký tự.")]
        public string? Genre { get; set; }

        [Display(Name = "Đạo Diễn")]
        [StringLength(255, ErrorMessage = "Tên đạo diễn không được vượt quá 255 ký tự.")]
        public string? Director { get; set; }

        [Display(Name = "Thời Gian (phút)")]
        [Range(1, int.MaxValue, ErrorMessage = "Thời gian phải lớn hơn 0 phút.")]
        public int? Duration { get; set; }

        [Display(Name = "Mô Tả")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Đánh Giá")]
        [StringLength(10, ErrorMessage = "Đánh giá không được vượt quá 10 ký tự.")]
        public string? Rating { get; set; }

        [Display(Name = "Ngày Công Chiếu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly? ReleaseDate { get; set; }

        [Display(Name = "Ảnh Poster")]
        public string? Poster { get; set; } // Holds the current poster URL

        [Display(Name = "Tải Lên Ảnh Poster")]
        [DataType(DataType.Upload)]
      
        public IFormFile? PosterFile { get; set; } // For the uploaded image file

        [Display(Name = "URL Trailer")]
        [Url(ErrorMessage = "Vui lòng nhập một URL hợp lệ.")]
        [StringLength(255, ErrorMessage = "URL không được vượt quá 255 ký tự.")]
        public string? TrailerUrl { get; set; }
    }
}
