using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBooking.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public async Task<IActionResult> Logout()
        {
            // Đăng xuất và xóa tất cả các cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Chuyển hướng về trang chủ hoặc trang đăng nhập
            return RedirectToAction("Index", "Home"); // Hoặc "Login" nếu bạn muốn chuyển tới trang đăng nhập
        }
    }
}
