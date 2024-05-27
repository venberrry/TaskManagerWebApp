namespace WebAppSummerSchool.Models
{
    public class UserObject
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public List<TaskObject> Tasks { get; set; }
    }
}