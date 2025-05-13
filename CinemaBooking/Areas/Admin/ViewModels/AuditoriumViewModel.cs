using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Areas.Admin.ViewModels
{
    public class AuditoriumViewModel
    {
        // No AuditoriumId here - it's auto-generated

        [Required(ErrorMessage = "Vui lòng chọn một rạp chiếu phim.")]
        [Display(Name = "Rạp Chiếu Phim")]
        public int CinemaId { get; set; } // Now required 

        [Required(ErrorMessage = "Vui lòng nhập tên phòng chiếu.")]
        [StringLength(100, ErrorMessage = "Tên phòng chiếu không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên phòng chiếu")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập sức chứa ghế.")]
        [Range(1, int.MaxValue, ErrorMessage = "Sức chứa ghế phải lớn hơn 0.")]
        [Display(Name = "Sức Chứa Ghế")]
        public int SeatCapacity { get; set; }
    }
}
