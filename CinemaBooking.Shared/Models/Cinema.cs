using System;
using System.Collections.Generic;

namespace CinemaBooking.Shared.Models;

public partial class Cinema
{
    public int CinemaId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public string? PhoneNumber { get; set; }

    public string? City { get; set; }

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();
}
