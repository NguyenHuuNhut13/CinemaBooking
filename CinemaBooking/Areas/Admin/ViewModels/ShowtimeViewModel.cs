using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Areas.Admin.ViewModels
{
    public class ShowtimeViewModel
    {
        public int ShowtimeId { get; set; }

        [Display(Name = "Phim")]
        [Required(ErrorMessage = "Vui lòng chọn phim.")]
        public int? MovieId { get; set; }

        [Display(Name = "Rạp Chiếu")]
        [Required(ErrorMessage = "Vui lòng chọn rạp chiếu.")]
        public int? AuditoriumId { get; set; }

        [Display(Name = "Ngày Chiếu")]
        [Required(ErrorMessage = "Vui lòng chọn ngày chiếu.")]
        public DateOnly? ShowDate { get; set; }

        [Display(Name = "Giờ Chiếu")]
        [Required(ErrorMessage = "Vui lòng nhập giờ chiếu.")]
        public TimeSpan ShowTime { get; set; } // Represents the showtime
    }
}
