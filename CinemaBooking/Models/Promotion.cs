using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Add this for the Display attribute

namespace CinemaBooking.Models
{
    public partial class Promotion
    {
        public int PromotionId { get; set; }

        [Display(Name = "Mã Khuyến Mãi")]
        public string Code { get; set; } = null!;

        [Display(Name = "Giảm Giá")]
        public decimal? Discount { get; set; }

        [Display(Name = "Ngày Bắt Đầu")]
        public DateOnly? StartDate { get; set; }

        [Display(Name = "Ngày Kết Thúc")]
        public DateOnly? EndDate { get; set; }
    }
}
