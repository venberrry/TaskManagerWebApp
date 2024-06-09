using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using WebAppSummerSchool.Models;
using WebAppSummerSchool.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppSummerSchool.DTO;
using System;

namespace WebAppSummerSchool.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly ITaskService _taskService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;

        public ProfileController(ILogger<ProfileController> logger,
            ITaskService taskService,
            IFileService fileService,
            ApplicationDbContext dbContext,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _taskService = taskService;
            _dbContext = dbContext;
            _fileService = fileService;
            _env = env;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Profile index page accessed.");

            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("Access attempt to Profile Index by unauthenticated user.");
                return RedirectToAction("Index", "Main");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                _logger.LogError("User ID claim is missing for authenticated user.");
                return RedirectToAction("Index", "Main");
            }
            var userId = int.Parse(userIdClaim.Value);

            var user = _dbContext.UserObject.Find(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var tasks = _dbContext.TaskObject
                .Where(t => t.UserObject.Role == user.Role)
                .ToList();

            ViewBag.CurrentEmail = user.Email; // Добавляем email пользователя в ViewBag для передачи в представление
            ViewBag.ImagePath = user.ImagePath;
            
            return View("~/Views/Profile/Profile.cshtml", tasks);
        }
        
        [HttpGet("CreateTask")]
        public IActionResult CreateTask()
        {
            _logger.LogInformation("Redirecting to Create Task page.");
            return View("~/Views/Profile/CreateTask.cshtml");
        }

        [HttpPost("CreateTask")]
        public IActionResult CreateTaskPost([FromForm] TaskObject task)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _dbContext.UserObject.Find(userId);

            var newTask = new TaskObject
            {
                Headline = task.Headline,
                Description = task.Description,
                Date = DateTime.SpecifyKind(task.Date, DateTimeKind.Utc),
                UserId = userId,
                UserObject = user
            };

            if (!TryValidateModel(newTask))
            {
                _logger.LogWarning("Model state is invalid");
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        _logger.LogWarning($"Validation error in {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                return View("CreateTask", newTask);
            }

            _dbContext.TaskObject.Add(newTask);
            _dbContext.SaveChanges();

            _logger.LogInformation("Task created successfully.");

            return RedirectToAction("Index");
        }


        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Starting logout process.");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Main");
        }
        
        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromForm] EmailDTO model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _dbContext.UserObject.FindAsync(userId);
            if (user == null)
            {
                return Content("Чет не то");
            }
            
            user.Email = model.Email;
            _dbContext.Entry(user).Property(u => u.Email).IsModified = true;
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        //Просто есть
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadStop(IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                ViewBag.Error = "Не авторизован";
                return View("Index");
            }

            var user = await _dbContext.UserObject.FindAsync(int.Parse(userId));
            if (user == null)
            {
                ViewBag.Error = "Пользователь не найден";
                return View("Index");
            }

            var filePath = await _fileService.UploadFileAsync(file, _env);
            if (!string.IsNullOrEmpty(filePath))
            {
                user.ImagePath = filePath; // Обновление пути изображения в профиле пользователя
                _dbContext.SaveChanges();
                ViewBag.Message = "Файл успешно загружен";
            }
            else
            {
                ViewBag.Error = "Ошибка загрузки файла";
            }

            return RedirectToAction("Index");
        }
    }
}
