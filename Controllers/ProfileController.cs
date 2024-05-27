using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Win32;

namespace WebAppSummerSchool.Controllers
{
    [Authorize] // Требуется авторизация для доступа к странице профиля
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Profile/Index.cshtml");
        }

        public IActionResult CreateTask()
        {
            // Перенаправление на страницу создания задачи
            return RedirectToAction("Create", "Task");
        }

        //Чтоб не тормозил основной поток обработки запросов
        public async Task<IActionResult> Logout()
        {
            // Выход из системы
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Main");
        }
    }
}
