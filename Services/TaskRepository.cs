using WebAppSummerSchool.Models;

namespace WebAppSummerSchool.Services
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<TaskObject> GetAllTasks()
        {
            return _dbContext.TaskObject.ToList();
        }

        public TaskObject GetTaskById(int taskId)
        {
            return _dbContext.TaskObject.Find(taskId);
        }

        public TaskObject AddTask(TaskObject task)
        {
            _dbContext.TaskObject.Add(task);
            _dbContext.SaveChanges();
            return task;
        }

        public TaskObject UpdateTask(TaskObject task)
        {
            _dbContext.TaskObject.Update(task);
            _dbContext.SaveChanges();
            return task;
        }

        public bool DeleteTask(int taskId)
        {
            var task = _dbContext.TaskObject.Find(taskId);
            if (task == null)
            {
                return false;
            }

            _dbContext.TaskObject.Remove(task);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
