using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Areas.Admin.ViewModels
{
    public class SeatViewModel
    {
        public int SeatId { get; set; }

        [Required(ErrorMessage = "Phòng chiếu là bắt buộc.")]
        public int? AuditoriumId { get; set; }

        [Required(ErrorMessage = "Hàng ghế là bắt buộc.")]
        [StringLength(5, ErrorMessage = "Hàng ghế có thể dài tối đa 5 ký tự.")]
        [Display(Name = "Hàng ghế")]
        public string Row { get; set; }

        [Required(ErrorMessage = "Số ghế là bắt buộc.")]
        [Range(1, 100, ErrorMessage = "Số ghế phải từ 1 đến 100.")]
        [Display(Name = "Số ghế")]
        public int? Number { get; set; }

        public IEnumerable<SelectListItem>? Auditoriums { get; set; }
    }
}
