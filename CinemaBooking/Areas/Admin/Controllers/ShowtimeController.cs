using CinemaBooking.Areas.Admin.ViewModels;
using CinemaBooking.Data;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShowtimeController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public ShowtimeController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // GET: Showtimes
        public async Task<IActionResult> Index()
        {
            var showtimes = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .ToListAsync();
            return View(showtimes);
        }

        // GET: Showtimes/Create
        public IActionResult Create()
        {
            ViewBag.Movies = new SelectList(_context.Movies, "MovieId", "Title");
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "CinemaId", "Name"); // Add Cinemas for selection
            ViewBag.Auditoriums = new SelectList(Enumerable.Empty<SelectListItem>()); // Initialize empty for auditoriums
            return View();
        }


        // POST: Showtimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowtimeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var showtime = new Showtime
                {
                    MovieId = model.MovieId,
                    AuditoriumId = model.AuditoriumId,
                    ShowDate = model.ShowDate,
                    ShowTime1 = TimeOnly.FromTimeSpan(model.ShowTime) // Converting TimeSpan to TimeOnly
                };

                _context.Add(showtime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Cinemas = new SelectList(_context.Cinemas, "CinemaId", "Name");
            ViewBag.Movies = new SelectList(_context.Movies, "MovieId", "Title", model.MovieId);
            ViewBag.Auditoriums = new SelectList(_context.Auditoriums, "AuditoriumId", "Name", model.AuditoriumId);
            return View(model);
        }

        // GET: Showtimes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var showtime = await _context.Showtimes.Include(s => s.Auditorium).FirstOrDefaultAsync(s => s.ShowtimeId == id);
            if (showtime == null)
            {
                return NotFound();
            }

            var model = new ShowtimeViewModel
            {
                ShowtimeId = showtime.ShowtimeId,
                MovieId = showtime.MovieId,
                AuditoriumId = showtime.AuditoriumId,
                ShowDate = showtime.ShowDate, // Default to today's date if null
                ShowTime = showtime.ShowTime1?.ToTimeSpan() ?? TimeSpan.Zero // Ensure ShowTime is initialized
            };
            var auditoriums = _context.Auditoriums.Where(a => a.CinemaId == showtime.Auditorium.CinemaId).ToList();
            ViewBag.Movies = new SelectList(_context.Movies, "MovieId", "Title", model.MovieId);
            ViewBag.Auditoriums = new SelectList(auditoriums, "AuditoriumId", "Name", model.AuditoriumId);
            return View(model);
        }

        // POST: Showtimes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShowtimeViewModel model)
        {
            if (id != model.ShowtimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var showtime = await _context.Showtimes.FindAsync(id);
                    if (showtime == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    showtime.MovieId = model.MovieId;
                    showtime.AuditoriumId = model.AuditoriumId;
                    showtime.ShowDate = model.ShowDate;
                    showtime.ShowTime1 = TimeOnly.FromTimeSpan(model.ShowTime);

                    _context.Update(showtime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowtimeExists(model.ShowtimeId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Movies = new SelectList(_context.Movies, "MovieId", "Title", model.MovieId);
            ViewBag.Auditoriums = new SelectList(_context.Auditoriums, "AuditoriumId", "Name", model.AuditoriumId);
            return View(model);
        }
        // GET: Showtimes/GetAuditoriums
        public IActionResult GetAuditoriums(int cinemaId)
        {
            var auditoriums = _context.Auditoriums
                .Where(a => a.CinemaId == cinemaId) // Assuming there is a CinemaId in the Auditorium model
                .Select(a => new { a.AuditoriumId, a.Name })
                .ToList();

            return Json(auditoriums); // Return as JSON for AJAX
        }

        // POST: Showtimes/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime != null)
            {
                _context.Showtimes.Remove(showtime);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ShowtimeExists(int id)
        {
            return _context.Showtimes.Any(e => e.ShowtimeId == id);
        }
    }
}
