using System.ComponentModel.DataAnnotations;

namespace WebAppSummerSchool.DTO
{
    public class EmailDTO
    {
        [Required(ErrorMessage = "email обязателен")]
        public string Email { get; set; }
    }
}