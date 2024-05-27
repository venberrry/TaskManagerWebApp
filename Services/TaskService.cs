using System.Collections.Generic;
using WebAppSummerSchool;

namespace WebAppSummerSchool.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<TaskObject> GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        public TaskObject GetTaskById(int taskId)
        {
            return _taskRepository.GetTaskById(taskId);
        }

        public TaskObject CreateTask(TaskObject task)
        {
            return _taskRepository.AddTask(task);
        }

        public TaskObject UpdateTask(int taskId, TaskObject task)
        {
            // Проверка наличия задачи
            var existingTask = _taskRepository.GetTaskById(taskId);
            if (existingTask == null)
            {
                return null;
            }

            // Обновление данных задачи
            existingTask.Headline = task.Headline;
            existingTask.Description = task.Description;
            existingTask.Date = task.Date;

            // Сохранение изменений
            return _taskRepository.UpdateTask(existingTask);
        }

        public bool DeleteTask(int taskId)
        {
            return _taskRepository.DeleteTask(taskId);
        }
    }
}