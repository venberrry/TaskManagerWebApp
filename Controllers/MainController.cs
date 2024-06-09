using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppSummerSchool.DTO;
using Microsoft.EntityFrameworkCore;
using WebAppSummerSchool.Models;

namespace WebAppSummerSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProfileController> _logger;


        public MainController(ApplicationDbContext dbContext, ILogger<ProfileController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Main/Main.cshtml");
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("~/Views/Main/Login.cshtml");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginPost([FromForm] LoginDTO model)
        {
            _logger.LogInformation("Received Username: {Username}, Password: {Password}", model.Username, model.Password);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                return View("Login", model);
            }

            try
            {
                var user = await _dbContext.UserObject.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

                if (user == null)
                {
                    _logger.LogWarning("User not found in database.");
                    ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
                    return View("Login", model);
                }

                _logger.LogInformation("User found: {Username}", user.Username);

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Добавляем идентификатор пользователя
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                _logger.LogInformation("User {Username} signed in successfully.", user.Username);

                return RedirectToAction("Index", "Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while logging in.");
                ModelState.AddModelError("", "Произошла ошибка при попытке входа.");
                return View("Login", model);
            }
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View("~/Views/Main/Register.cshtml");
        }


            [HttpPost("Register")]
        public async Task<IActionResult> RegisterPost([FromForm] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            var existingUser = await _dbContext.UserObject.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "Пользователь с таким именем уже существует");
                return View("Register", model);
            }

            var user = new UserObject
            {
                Role = model.Role,
                Username = model.Username,
                Password = model.Password,
                Email = "none"  // Устанавливаем email в 'none' при регистрации
            };

            _dbContext.UserObject.Add(user);
            await _dbContext.SaveChangesAsync();

            // Аутентификация пользователя после успешной регистрации
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Profile");
        }
    }
}
