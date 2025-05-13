using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Models
{
    public partial class Movie
    {
        public int MovieId { get; set; }

        [Display(Name = "Tiêu Đề")]
        public string Title { get; set; } = null!;

        [Display(Name = "Thể Loại")]
        public string? Genre { get; set; }

        [Display(Name = "Đạo Diễn")]
        public string? Director { get; set; }

        [Display(Name = "Thời Gian (phút)")]
        public int? Duration { get; set; }

        [Display(Name = "Mô Tả")]
        public string? Description { get; set; }

        [Display(Name = "Đánh Giá")]
        public string? Rating { get; set; }

        [Display(Name = "Ngày Công Chiếu")]
        public DateOnly? ReleaseDate { get; set; }

        [Display(Name = "Ảnh Poster")]
        public string? Poster { get; set; }

        [Display(Name = "URL Trailer")]
        public string? TrailerUrl { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
