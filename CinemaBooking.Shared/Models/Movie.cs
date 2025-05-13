using System;
using System.Collections.Generic;

namespace CinemaBooking.Shared.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public string? Genre { get; set; }

    public string? Director { get; set; }

    public int? Duration { get; set; }

    public string? Description { get; set; }

    public string? Rating { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public string? Poster { get; set; }

    public string? TrailerUrl { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
