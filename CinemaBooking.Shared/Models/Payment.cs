using System;
using System.Collections.Generic;

namespace CinemaBooking.Shared.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? TicketId { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
