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

namespace WebAppSummerSchool.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly ITaskService _taskService;
        private readonly ApplicationDbContext _dbContext;
        private readonly EmailService _emailService;
        private readonly FileService _fileService;

        public ProfileController(ILogger<ProfileController> logger, ITaskService taskService, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _taskService = taskService;
            _dbContext = dbContext;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            _logger.LogInformation("Profile index page accessed.");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userRole = User.FindFirst(ClaimTypes.Role).Value;

            var tasks = _dbContext.TaskObject
                .Where(t => t.UserObject.Role == userRole)
                .ToList();

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
                return NotFound();
            }

            user.Email = model.Email;
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var filePath = await _fileService.UploadFileAsync(file);
            if (filePath != null)
            {
                ViewBag.Message = "Загрузили файл";
            }
            else
            {
                ViewBag.Message = "Не загружен файл";
            }
            return View();
        }
    }
}
