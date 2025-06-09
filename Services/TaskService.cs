using Microsoft.EntityFrameworkCore;
using StudentCouncilAPI.Data;
using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _context.Tasks
                .Include(t => t.Event)
                .Include(t => t.Partner)
                .Include(t => t.TaskUsers)
                .ThenInclude(tu => tu.User)
                .OrderByDescending(t => t.StartDate)
                .ToListAsync();
        }

        public async Task<Models.Task?> GetTaskByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Event)
                .Include(t => t.Partner)
                .Include(t => t.TaskUsers)
                .ThenInclude(tu => tu.User)
                .FirstOrDefaultAsync(t => t.TaskId == id);
        }

        public async Task<Models.Task> CreateTaskAsync(Models.Task task)
        {
            task.TaskId = Guid.NewGuid();
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Models.Task> UpdateTaskAsync(Models.Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await GetTaskByIdAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Models.Task>> GetTasksByEventAsync(Guid eventId)
        {
            return await _context.Tasks
                .Where(t => t.EventId == eventId)
                .Include(t => t.Partner)
                .Include(t => t.TaskUsers)
                .ThenInclude(tu => tu.User)
                .OrderBy(t => t.EndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Task>> GetTasksByUserAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.TaskUsers.Any(tu => tu.UserId == userId))
                .Include(t => t.Event)
                .Include(t => t.Partner)
                .Include(t => t.TaskUsers)
                .ThenInclude(tu => tu.User)
                .OrderBy(t => t.EndDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Models.Task>> GetTasksByStatusAsync(Models.TaskStatus status)
        {
            return await _context.Tasks
                .Where(t => t.Status == status)
                .Include(t => t.Event)
                .Include(t => t.Partner)
                .Include(t => t.TaskUsers)
                .ThenInclude(tu => tu.User)
                .OrderBy(t => t.EndDate)
                .ToListAsync();
        }

        public async Task<bool> AssignTaskToUserAsync(Guid taskId, Guid userId, TaskUserRole role)
        {
            var existingAssignment = await _context.TaskUsers
                .FirstOrDefaultAsync(tu => tu.TaskId == taskId && tu.UserId == userId);

            if (existingAssignment != null)
            {
                existingAssignment.Role = role;
            }
            else
            {
                _context.TaskUsers.Add(new TaskUser
                {
                    TaskId = taskId,
                    UserId = userId,
                    Role = role
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromTaskAsync(Guid taskId, Guid userId)
        {
            var assignment = await _context.TaskUsers
                .FirstOrDefaultAsync(tu => tu.TaskId == taskId && tu.UserId == userId);

            if (assignment == null) return false;

            _context.TaskUsers.Remove(assignment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
