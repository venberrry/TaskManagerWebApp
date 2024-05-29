using System.ComponentModel.DataAnnotations;

namespace WebAppSummerSchool.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
    }
}