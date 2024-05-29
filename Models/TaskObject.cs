using WebAppSummerSchool.Models;

namespace WebAppSummerSchool
{
    public class TaskObject
    {
        public int Id { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public UserObject? UserObject { get; set; }

    }
}
