using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaBooking.Data;
using System.Linq;
using System.Threading.Tasks;
using CinemaBooking.Areas.Admin.ViewModels;

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")] // Important: Specify the area
    public class SeatController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public SeatController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // GET: Seat
        public async Task<IActionResult> Index(int? cinemaId, int? auditoriumId)
        {
            // Load cinemas for the dropdown list
            ViewBag.Cinemas = await _context.Cinemas
                .Select(c => new SelectListItem
                {
                    Value = c.CinemaId.ToString(),
                    Text = c.Name
                }).ToListAsync();

            // Load auditoriums for the dropdown list, filter by cinema if provided
            ViewBag.Auditoriums = cinemaId.HasValue
                ? await _context.Auditoriums
                    .Where(a => a.CinemaId == cinemaId)
                    .Select(a => new SelectListItem
                    {
                        Value = a.AuditoriumId.ToString(),
                        Text = a.Name
                    }).ToListAsync()
                : new List<SelectListItem>();

            // Get all seats and include the related auditorium and cinema
            var seatsQuery = _context.Seats
                .Include(s => s.Auditorium)
                .ThenInclude(a => a.Cinema)
                .AsQueryable();

            // Filter by cinemaId if provided
            if (cinemaId.HasValue)
            {
                seatsQuery = seatsQuery.Where(s => s.Auditorium.CinemaId == cinemaId.Value);
                ViewData["SelectedCinemaId"] = cinemaId;
            }

            // Filter by auditoriumId if provided
            if (auditoriumId.HasValue)
            {
                seatsQuery = seatsQuery.Where(s => s.AuditoriumId == auditoriumId.Value);
                ViewData["SelectedAuditoriumId"] = auditoriumId;
            }

            var seats = await seatsQuery.ToListAsync();

            return View(seats);
        }
        public async Task<IActionResult> GetAuditoriums(int cinemaId)
        {
            var auditoriums = await _context.Auditoriums
                .Where(a => a.CinemaId == cinemaId)
                .Select(a => new { value = a.AuditoriumId, text = a.Name })
                .ToListAsync();

            return Json(auditoriums);
        }

        // GET: Seat/Create
        public async Task<IActionResult> Create()
        {
            var model = new SeatViewModel
            {
                Auditoriums = _context.Auditoriums.Select(a => new SelectListItem
                {
                    Value = a.AuditoriumId.ToString(),
                    Text = a.Name
                }).ToList()
            };
            // Load cinemas for the dropdown list
            ViewBag.Cinemas = await _context.Cinemas
                .Select(c => new SelectListItem
                {
                    Value = c.CinemaId.ToString(),
                    Text = c.Name
                }).ToListAsync();
            return View(model);
        }

        // POST: Seat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeatViewModel seatViewModel)
        {
            if (ModelState.IsValid)
            {
                var seat = new Seat
                {
                    AuditoriumId = seatViewModel.AuditoriumId,
                    Row = seatViewModel.Row,
                    Number = seatViewModel.Number
                };
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            seatViewModel.Auditoriums = _context.Auditoriums.Select(a => new SelectListItem
            {
                Value = a.AuditoriumId.ToString(),
                Text = a.Name
            }).ToList();
            // Load cinemas for the dropdown list
            ViewBag.Cinemas = await _context.Cinemas
                .Select(c => new SelectListItem
                {
                    Value = c.CinemaId.ToString(),
                    Text = c.Name
                }).ToListAsync();
            return View(seatViewModel);
        }

        // GET: Seat/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var seat = await _context.Seats
                              .Include(s => s.Auditorium) // Eager load related Auditorium data
                              .FirstOrDefaultAsync(s => s.SeatId == id); // Use FirstOrDefaultAsync for a more explicit query

            if (seat == null)
            {
                return NotFound();
            }

            var model = new SeatViewModel
            {
                SeatId = seat.SeatId,
                AuditoriumId = seat.AuditoriumId,
                Row = seat.Row,
                Number = seat.Number,
                Auditoriums = _context.Auditoriums
            .Where(a => a.CinemaId == seat.Auditorium.CinemaId) // Filter auditoriums based on the seat's auditorium's cinema
            .Select(a => new SelectListItem
            {
                Value = a.AuditoriumId.ToString(),
                Text = a.Name
            }).ToList()
            };
   
            return View(model);
        }

        // POST: Seat/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SeatViewModel seatViewModel)
        {
            if (id != seatViewModel.SeatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var seat = new Seat
                    {
                        SeatId = seatViewModel.SeatId,
                        AuditoriumId = seatViewModel.AuditoriumId,
                        Row = seatViewModel.Row,
                        Number = seatViewModel.Number
                    };
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatExists(seatViewModel.SeatId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Reload auditoriums based on the selected auditorium's cinema
            var selectedAuditorium = _context.Auditoriums
                .FirstOrDefault(a => a.AuditoriumId == seatViewModel.AuditoriumId);

            if (selectedAuditorium != null)
            {
                seatViewModel.Auditoriums = _context.Auditoriums
                    .Where(a => a.CinemaId == selectedAuditorium.CinemaId) // Filter auditoriums based on the cinema of the selected auditorium
                    .Select(a => new SelectListItem
                    {
                        Value = a.AuditoriumId.ToString(),
                        Text = a.Name
                    }).ToList();
            }


            return View(seatViewModel);
        }

        // GET: Seat/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var seat = await _context.Seats
                .Include(s => s.Auditorium)
                .FirstOrDefaultAsync(m => m.SeatId == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat != null)
            {
                _context.Seats.Remove(seat);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(int id)
        {
            return _context.Seats.Any(e => e.SeatId == id);
        }
    }
}
