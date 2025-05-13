using System;
using System.Collections.Generic;

namespace CinemaBooking.Shared.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? AuditoriumId { get; set; }

    public string? Row { get; set; }

    public int? Number { get; set; }

    public virtual Auditorium? Auditorium { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
