using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace CinemaBooking.Models
{
    public partial class Seat
    {
        public int SeatId { get; set; }

        [Display(Name = "Phòng Chiếu")]
        public int? AuditoriumId { get; set; }

        [Display(Name = "Hàng")]
        public string? Row { get; set; }

        [Display(Name = "Số Ghế")]
        public int? Number { get; set; }

        public virtual Auditorium? Auditorium { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
