using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppSummerSchool.Models;

namespace WebAppSummerSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;

        public MainController(IConfiguration configuration)
        {
            _configuration = configuration;
            ApplicationDbContext applicationDbContext = new ApplicationDbContext();
            _dbContext = applicationDbContext;
            _jwtTokenGenerator = new JwtTokenGenerator(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/Main/Main.cshtml");
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("~/Views/Main/Login.cshtml", new LoginViewModel());
        }

        [HttpPost("Login")]
        public IActionResult LoginPost([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", model);
            }

            var user = _dbContext.UserObject.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
                return View("Login", model);
            }

            var token = _jwtTokenGenerator.GenerateToken(user.Username, user.Role);

            return RedirectToAction("Index", "Profile");
        }
    

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View("~/Views/Main/Register.cshtml");
        }

        [HttpPost("Register")]
        public IActionResult RegisterPost([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            var user = new UserObject
            {
                Role = model.Role,
                Username = model.Username,
                Password = model.Password
            };

            _dbContext.UserObject.Add(user);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Profile");
        }


    }
}
