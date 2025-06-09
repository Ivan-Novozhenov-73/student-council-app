using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<Models.Task>> GetAllTasksAsync();
        Task<Models.Task?> GetTaskByIdAsync(Guid id);
        Task<Models.Task> CreateTaskAsync(Models.Task task);
        Task<Models.Task> UpdateTaskAsync(Models.Task task);
        Task<bool> DeleteTaskAsync(Guid id);
        Task<IEnumerable<Models.Task>> GetTasksByEventAsync(Guid eventId);
        Task<IEnumerable<Models.Task>> GetTasksByUserAsync(Guid userId);
        Task<IEnumerable<Models.Task>> GetTasksByStatusAsync(Models.TaskStatus status);
        Task<bool> AssignTaskToUserAsync(Guid taskId, Guid userId, TaskUserRole role);
        Task<bool> RemoveUserFromTaskAsync(Guid taskId, Guid userId);
    }
}
