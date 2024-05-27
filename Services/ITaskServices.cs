using System.Collections.Generic;
using WebAppSummerSchool;

// методы для выполнения CRUD операций
public interface ITaskService
{
    IEnumerable<TaskObject> GetAllTasks();
    TaskObject GetTaskById(int taskId);
    TaskObject CreateTask(TaskObject task);
    TaskObject UpdateTask(int taskId, TaskObject task);
    bool DeleteTask(int taskId);
}