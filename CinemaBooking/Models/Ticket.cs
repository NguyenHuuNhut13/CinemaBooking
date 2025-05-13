using System;
using System.Collections.Generic;

namespace CinemaBooking.Models;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int? ShowtimeId { get; set; }

    public int? SeatId { get; set; }

    public int? UserId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? BookingTime { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Seat? Seat { get; set; }

    public virtual Showtime? Showtime { get; set; }

    public virtual User? User { get; set; }
}
