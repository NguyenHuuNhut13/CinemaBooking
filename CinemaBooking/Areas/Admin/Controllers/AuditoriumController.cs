using CinemaBooking.Data;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using CinemaBooking.Areas.Admin.ViewModels; // For SelectList

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuditoriumController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public AuditoriumController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? cinemaId)
        {
            var cinemas = await _context.Cinemas.ToListAsync();
            ViewData["CinemaId"] = new SelectList(cinemas, "CinemaId", "Name", cinemaId);

            var auditoriums = _context.Auditoriums.Include(a => a.Cinema).AsQueryable();

            if (cinemaId.HasValue)
            {
                auditoriums = auditoriums.Where(a => a.CinemaId == cinemaId);
            }

            return View(await auditoriums.ToListAsync());
        }



        // GET: Admin/Auditorium/Create
        public IActionResult Create()
        {
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "CinemaId", "Name");
            return View();
        }

        // POST: Admin/Auditorium/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuditoriumViewModel auditoriumViewModel)
        {
            if (ModelState.IsValid)
            {
                var auditorium = new Auditorium
                {
                    CinemaId = auditoriumViewModel.CinemaId,
                    Name = auditoriumViewModel.Name,
                    SeatCapacity = auditoriumViewModel.SeatCapacity
                };

                _context.Add(auditorium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "CinemaId", "Name", auditoriumViewModel.CinemaId);
            return View(auditoriumViewModel);
        }

        // GET: Admin/Auditorium/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditorium = await _context.Auditoriums.FindAsync(id);
            if (auditorium == null)
            {
                return NotFound();
            }

            var auditoriumViewModel = new AuditoriumViewModel
            {
                CinemaId = auditorium.CinemaId ?? 0, // Handle possible null
                Name = auditorium.Name ?? "",        // Handle possible null
                SeatCapacity = auditorium.SeatCapacity ?? 0 // Handle possible null
            };

            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "CinemaId", "Name", auditoriumViewModel.CinemaId);
            return View(auditoriumViewModel);
        }

        // POST: Admin/Auditorium/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AuditoriumViewModel auditoriumViewModel)
        {
            // Remove the ID check, as the ViewModel doesn't contain AuditoriumId
            // if (id != auditoriumViewModel.AuditoriumId) 
            // {
            //     return NotFound(); 
            // }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing auditorium from the database using id from the route
                    var auditorium = await _context.Auditoriums.FindAsync(id);
                    if (auditorium == null)
                    {
                        return NotFound();
                    }

                    // Update the auditorium entity with values from the view model
                    auditorium.CinemaId = auditoriumViewModel.CinemaId;
                    auditorium.Name = auditoriumViewModel.Name;
                    auditorium.SeatCapacity = auditoriumViewModel.SeatCapacity;

                    _context.Update(auditorium);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle concurrency (if needed)
                    throw;
                }
            }
            // If ModelState is invalid or an error occurs, repopulate the dropdown and return to the edit view
            ViewData["CinemaId"] = new SelectList(_context.Cinemas, "CinemaId", "Name", auditoriumViewModel.CinemaId);
            return View(auditoriumViewModel);
        }
        // POST: Admin/Auditorium/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var auditorium = await _context.Auditoriums.FindAsync(id);
            if (auditorium != null)
            {
                _context.Auditoriums.Remove(auditorium);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AuditoriumExists(int id)
        {
            return _context.Auditoriums.Any(e => e.AuditoriumId == id);
        }
    }
}