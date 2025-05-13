using System;
using System.Collections.Generic;

namespace CinemaBooking.Shared.Models;

public partial class Auditorium
{
    public int AuditoriumId { get; set; }

    public int? CinemaId { get; set; }

    public string? Name { get; set; }

    public int? SeatCapacity { get; set; }

    public virtual Cinema? Cinema { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
