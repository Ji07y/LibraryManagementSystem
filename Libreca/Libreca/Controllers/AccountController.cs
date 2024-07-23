using Microsoft.AspNetCore.Mvc;
using Libreca.Data;
using Libreca.Models;
using NotificationService;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Libreca.Controllers
{
    public class AccountController : Controller
    {
        private readonly LibraryContext _context;
        private readonly RabbitMQService _rabbitMQService;

        public AccountController(LibraryContext context, RabbitMQService rabbitMQService)
        {
            _context = context;
            _rabbitMQService = rabbitMQService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user == null || user.Password != model.Password)
                {
                    ModelState.AddModelError("", "El nombre de usuario o la contraseña son incorrectos.");
                    return View(model);
                }

                // Aquí se establece el cookie de autenticación.
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, model.Username)
        };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Books");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el correo electrónico o nombre de usuario ya existe
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "El correo electrónico ya está registrado.");
                    return View(model);
                }

                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "El nombre de usuario ya está en uso.");
                    return View(model);
                }

                // Guardar el usuario en la base de datos
                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                // Enviar correo de confirmación usando RabbitMQ
                var message = $"Thank you for registering, {model.Username}!";
                _rabbitMQService.SendEmail(model.Email, "Registration Confirmation", message);

                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
