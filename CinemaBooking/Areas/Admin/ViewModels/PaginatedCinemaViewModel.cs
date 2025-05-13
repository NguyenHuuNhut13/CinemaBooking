using CinemaBooking.Models;

namespace CinemaBooking.Areas.Admin.ViewModels
{
    public class PaginatedCinemaViewModel
    {
        public IEnumerable<Cinema> Cinemas { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}
