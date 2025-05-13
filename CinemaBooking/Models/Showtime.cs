using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Import for Display attribute

namespace CinemaBooking.Models
{
    public partial class Showtime
    {
        public int ShowtimeId { get; set; }

        [Display(Name = "Mã Phim")]
        public int? MovieId { get; set; }

        [Display(Name = "Phòng Chiếu")]
        public int? AuditoriumId { get; set; }

        [Display(Name = "Ngày Chiếu")]
        public DateOnly? ShowDate { get; set; }

        [Display(Name = "Giờ Chiếu")]
        public TimeOnly? ShowTime1 { get; set; }

        public virtual Auditorium? Auditorium { get; set; }

        public virtual Movie? Movie { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
