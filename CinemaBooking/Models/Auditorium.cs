using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CinemaBooking.Models
{
    public partial class Auditorium
    {
        [DisplayName("Mã Phòng")]
        public int AuditoriumId { get; set; }
        [DisplayName("Mã rạp")]
        public int? CinemaId { get; set; }

        [DisplayName("Tên Phòng")] 
        public string? Name { get; set; }

        [DisplayName("Sức Chứa Ghế")]
        public int? SeatCapacity { get; set; }

        [DisplayName("Rạp Phim")]
        public virtual Cinema? Cinema { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
