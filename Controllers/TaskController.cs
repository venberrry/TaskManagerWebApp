using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppSummerSchool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskController(ApplicationDbContext dbContext)
        {
            //ApplicationDbContext applicationDbContext = new ApplicationDbContext();

            //_dbContext = applicationDbContext;
            _dbContext = dbContext;
        }

        [HttpGet("List")]
        public IActionResult Get()
        {
            var tasks = _dbContext.TaskObject.ToList();
            return View("~/Views/Task/List.cshtml", tasks);
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var task = _dbContext.TaskObject.Find(id);
            if (task == null)
            {
                return NotFound();
            }
            return View("~/Views/Task/Details.cshtml", task);
        }

        [HttpGet("Form")]
        public IActionResult PostPage()
        {
            return View("~/Views/Task/Form.cshtml");
        }

        [HttpPost("Form")]
        public IActionResult Post([FromForm] TaskObject task)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTask = new TaskObject
            {
                Headline = task.Headline,
                Description = task.Description,
                Date = DateTime.SpecifyKind(task.Date, DateTimeKind.Utc)
            };

            _dbContext.TaskObject.Add(newTask);
            _dbContext.SaveChanges();

            return Redirect("List");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TaskObject task)
        {
            var existingTask = _dbContext.TaskObject.Find(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            existingTask.Headline = task.Headline;
            existingTask.Description = task.Description;
            existingTask.Date = task.Date;

            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _dbContext.TaskObject.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            _dbContext.TaskObject.Remove(task);
            _dbContext.SaveChanges();

            return NoContent();
        }

    }
}
