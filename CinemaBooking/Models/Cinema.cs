using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Models
{
    public partial class Cinema
    {
        [Display(Name = "Mã Rạp")]
        public int CinemaId { get; set; }

        [Display(Name = "Tên Rạp")]
        public string Name { get; set; } = null!;

        [Display(Name = "Địa Điểm")]
        public string? Location { get; set; }

        [Display(Name = "Số Điện Thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Thành Phố")]
        public string? City { get; set; }

        public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();
    }
}
