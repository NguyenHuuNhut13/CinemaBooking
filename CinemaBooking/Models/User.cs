using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace CinemaBooking.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        [Display(Name = "Tên Người Dùng")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Mật Khẩu")]
        public string PasswordHash { get; set; } = null!;

        [Display(Name = "Số Điện Thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Vai Trò")]
        public string? Role { get; set; }

        [Display(Name = "Ngày Tạo")]
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
