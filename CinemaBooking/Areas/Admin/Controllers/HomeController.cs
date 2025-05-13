using Microsoft.AspNetCore.Mvc;
using CinemaBooking.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public HomeController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Statistic: Total count of each entity
            var totalCinemas = await _context.Cinemas.CountAsync();
            var totalMovies = await _context.Movies.CountAsync();
            var totalShowtimes = await _context.Showtimes.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var totalTickets = await _context.Tickets.CountAsync();

            // Statistic: Total revenue calculation
            var totalRevenue = await _context.Payments.SumAsync(p => p.Amount);

            // Statistic: Top best-selling movies by ticket count
            var topMovies = await _context.Tickets
                .Include(t => t.Showtime)
                .ThenInclude(s => s.Movie)
                .GroupBy(t => t.Showtime.Movie.Title)
                .Select(g => new
                {
                    MovieTitle = g.Key,
                    TicketsSold = g.Count(),
                    TotalRevenue = g.Sum(t => t.Price)
                })
                .OrderByDescending(m => m.TicketsSold)
                .Take(5)
                .ToListAsync();

            // Prepare data to be passed to the view
            ViewBag.TotalCinemas = totalCinemas;
            ViewBag.TotalMovies = totalMovies;
            ViewBag.TotalShowtimes = totalShowtimes;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalTickets = totalTickets;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TopMovies = topMovies;

            return View();
        }
    }
}
