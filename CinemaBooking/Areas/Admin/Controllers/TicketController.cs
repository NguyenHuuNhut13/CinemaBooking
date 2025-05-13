using CinemaBooking.Data;
using CinemaBooking.Models;
using CinemaBooking.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketController : Controller
    {
        private readonly CinemaBookingDbContext _context;

        public TicketController(CinemaBookingDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Ticket
        public async Task<IActionResult> Index()
        {
            // Retrieve the list of tickets with related information like showtime, seat, and user.
            var tickets = await _context.Tickets
                .Include(t => t.Showtime)
                .Include(t => t.Seat)
                .Include(t => t.User)
                .ToListAsync();

            return View(tickets);
        }

        // GET: Admin/Ticket/Payment/5
        public async Task<IActionResult> Payment(int id)
        {
            // Retrieve the ticket by ID to process the payment.
            var ticket = await _context.Tickets
                .Include(t => t.Showtime)
                .Include(t => t.Seat)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            // Pass the ticket to the payment view
            return View(ticket);
        }


        // POST: Admin/Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket != null)
            {
                // Remove the ticket from the database
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        // POST: Process Payment for Ticket
        [HttpPost]
        public IActionResult ProcessPayment(int ticketId, decimal amount)
        {
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket == null)
            {
                return NotFound();
            }

            // Assuming you have a Payment model to record the payment details
            var payment = new Payment
            {
                TicketId = ticketId,
                Amount = amount,
                PaymentDate = DateTime.Now
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            // Optionally: You can also update the ticket status here
            ticket.Price = amount;
            _context.SaveChanges();

            // Return to the ticket list or show a success message
            return RedirectToAction(nameof(Index));
        }
    }
}
