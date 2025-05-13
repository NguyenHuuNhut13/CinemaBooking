using CinemaBooking.Data;
using CinemaBooking.Models;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Scripting;
using System.Linq;


namespace CinemaBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaBookingDbContext _context;

        public HomeController(ILogger<HomeController> logger, CinemaBookingDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }
        public IActionResult Details(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Showtimes)
                    .ThenInclude(s => s.Auditorium) 
                        .ThenInclude(a => a.Seats) 
                .Include(m => m.Reviews)
                    .ThenInclude(r => r.User)  
                .FirstOrDefault(m => m.MovieId == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(int movieId, int rating, string comment)
        {
            if (ModelState.IsValid)
            {
                var movie = _context.Movies
                    .Include(m => m.Reviews)
                    .FirstOrDefault(m => m.MovieId == movieId);

                if (movie != null)
                {
                    var review = new Review
                    {
                        Rating = rating,
                        Comment = comment,
                        ReviewDate = DateTime.Now,
                        UserId = int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) ? userId : null, 
                        MovieId = movie.MovieId
                    };


                    movie.Reviews.Add(review);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Cảm ơn bạn đã gửi đánh giá!";
                    return RedirectToAction("Details", new { id = movieId });
                }

                return NotFound();
            }

            TempData["ErrorMessage"] = "Có lỗi xảy ra khi gửi đánh giá.";
            return RedirectToAction("Details", new { id = movieId });
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == model.Email && u.PasswordHash == model.Password);
                if (user != null)
                {
                    // Create claims for the user
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Assuming UserID is your identifier
                new Claim(ClaimTypes.Role, user.Role) // Add role claim
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // This keeps the user signed in across sessions
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // Optional: Set expiration
                    };

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Check user role and redirect accordingly
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Cinema", new { area = "Admin" });
                    }

                    // Redirect to the default page for regular users
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }




        public IActionResult Register()
        {
            return View();
        }
        // POST: Register user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create new user
                var user = new User
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = model.Password,
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Redirect or log in the user
                return RedirectToAction("Index", "Home");
            }

            // If validation fails, re-render the registration page with errors
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookTickets(int showtimeId, int[] selectedSeats)
        {
            if (selectedSeats == null || !selectedSeats.Any())
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một ghế.");
                return RedirectToAction("Details", new { id = showtimeId });
            }

            var showtime = _context.Showtimes
                .Include(s => s.Auditorium)
                    .ThenInclude(a => a.Seats)
                .FirstOrDefault(s => s.ShowtimeId == showtimeId);

            if (showtime == null)
            {
                return NotFound();
            }

            var availableSeats = showtime.Auditorium.Seats.Where(s => selectedSeats.Contains(s.SeatId)).ToList();
            if (availableSeats.Count != selectedSeats.Length)
            {
                ModelState.AddModelError(string.Empty, "Một số ghế đã chọn không có sẵn.");
                return RedirectToAction("Details", new { id = showtime.MovieId });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy người dùng.");
                return RedirectToAction("Login");
            }

            // Parse userId as integer
            int parsedUserId;
            if (!int.TryParse(userId, out parsedUserId))
            {
                ModelState.AddModelError(string.Empty, "ID người dùng không hợp lệ.");
                return RedirectToAction("Login");
            }

            // Check if the user has already booked any of the selected seats
            var alreadyBookedSeats = _context.Tickets
                .Where(t => t.UserId == parsedUserId && selectedSeats.Contains((int)t.SeatId) && t.ShowtimeId == showtimeId)
                .Select(t => t.SeatId)
                .ToList();

            if (alreadyBookedSeats.Any())
            {
                var bookedSeatNumbers = string.Join(", ", alreadyBookedSeats);
                ModelState.AddModelError(string.Empty, $"Bạn đã đặt ghế: {bookedSeatNumbers} cho suất chiếu này.");
                return RedirectToAction("Details", new { id = showtimeId });
            }

            foreach (var seatId in selectedSeats)
            {
                var ticket = new Ticket
                {
                    ShowtimeId = showtimeId,
                    SeatId = seatId,
                    BookingTime = DateTime.Now,
                    UserId = parsedUserId
                };
                _context.Tickets.Add(ticket);
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Đặt vé thành công, 1 vé có giá là: 90.000!";

            return RedirectToAction("PaymentWithQR");
        }


        public IActionResult TicketHistory()
        {
            // Assuming you have a Ticket model and it is related to the user via UserId
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var ticketHistory = _context.Tickets
                .Where(t => t.UserId.ToString() == userId)
                .Include(t => t.Showtime)
                .ThenInclude(s => s.Movie)
                .ToList();

            return View(ticketHistory);
        }
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // If the search query is empty, show all movies
                var movies = _context.Movies.ToList();
                return View("Index", movies);  // Assuming you want to display the results on the Index view
            }

            // Otherwise, search for movies by name
            var searchResults = _context.Movies
                                        .Where(m => m.Title.Contains(query))  // Assuming "Name" is the property you want to search
                                        .ToList();

            return View("Index", searchResults);  // Pass the search results to the Index view
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> GetTicket(int ticketId)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Showtime)
                    .ThenInclude(s => s.Movie)
                .Include(t => t.User) // Nạp thông tin người dùng
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);

            if (ticket == null)
            {
                return NotFound();
            }

            var ticketDto = new
            {
                ticket.TicketId,
                ticket.SeatId,
                ticket.BookingTime,
                User = new // Thêm thông tin người dùng vào DTO
                {
                    ticket.User.UserId,
                    ticket.User.FullName,
                    ticket.User.Email
                },
                Showtime = new
                {
                    ticket.Showtime.ShowTime1,
                    Movie = new
                    {
                        ticket.Showtime.Movie.Title
                    }
                }
            };

            return Ok(ticketDto);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Fetch user ID from claims
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                ModelState.AddModelError("", "Không thể xác thực người dùng. Vui lòng đăng nhập lại.");
                return View(model);
            }

            // Retrieve user from the database
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                ModelState.AddModelError("", "Không tìm thấy người dùng.");
                return View(model);
            }

            // Set the new password (still plain-text as per the original code)
            user.PasswordHash = model.NewPassword;
            _context.SaveChanges();

            // Confirmation message
            ViewBag.Message = "Đổi mật khẩu thành công.";
            return RedirectToAction("Logout");
        }


        [HttpGet]
        public IActionResult PaymentWithQR()
        {
            // Return a view model with default values
            var model = new PaymentWithQRViewModel
            {
                Amount = 0 // Default amount
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult PaymentWithQR(PaymentWithQRViewModel model)
        {
            if (model.Amount <= 0)
            {
                TempData["ErrorMessage"] = "Vui lòng nhập số tiền hợp lệ."; // Please enter a valid amount.
                return View(model);
            }

            try
            {
                // Hardcoded details
                string hardcodedBankKey = BankKey.VIETCOMBANK; // Replace with actual bank key
                string hardcodedAccountNumber = "1026730941"; // Replace with actual account number
                string hardcodedPurpose = "Thanh toán dịch vụ"; // Replace with actual purpose

                // Generate QR content
                var qrPay = QRPay.InitVietQR(
                    bankBin: BankApp.BanksObject[hardcodedBankKey].bin,
                    bankNumber: hardcodedAccountNumber,
                    amount: model.Amount.ToString(),
                    purpose: hardcodedPurpose
                );

                var qrContent = qrPay.Build();

                // Pass the generated QR content to the view
                model.QRContent = qrContent;
                TempData["SuccessMessage"] = "Mã QR đã được tạo thành công."; // QR code generated successfully.
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}"; // An error occurred.
            }

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
