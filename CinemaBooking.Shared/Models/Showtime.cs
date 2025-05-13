using System;
using System.Collections.Generic;

namespace CinemaBooking.Shared.Models;

public partial class Showtime
{
    public int ShowtimeId { get; set; }

    public int? MovieId { get; set; }

    public int? AuditoriumId { get; set; }

    public DateOnly? ShowDate { get; set; }

    public TimeOnly? ShowTime1 { get; set; }

    public virtual Auditorium? Auditorium { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
