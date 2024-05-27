using WebAppSummerSchool;

namespace WebAppSummerSchool.Services
{
    // методы для доступа к данным о задачах в бд
    public interface ITaskRepository
    {
        IEnumerable<TaskObject> GetAllTasks();
        TaskObject GetTaskById(int taskId);
        TaskObject AddTask(TaskObject task);
        TaskObject UpdateTask(TaskObject task);
        bool DeleteTask(int taskId);
    }
}