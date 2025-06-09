using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.Services;
using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentCouncilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // Получить задачи (фильтрация по eventId, userId, статусу)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(
            [FromQuery] Guid? eventId = null,
            [FromQuery] Guid? userId = null,
            [FromQuery] TaskStatus? status = null)
        {
            IEnumerable<Models.Task> tasks;

            if (eventId.HasValue)
            {
                tasks = await _taskService.GetTasksByEventAsync(eventId.Value);
            }
            else if (userId.HasValue)
            {
                tasks = await _taskService.GetTasksByUserAsync(userId.Value);
            }
            else if (status.HasValue)
            {
                tasks = await _taskService.GetTasksByStatusAsync(status.Value);
            }
            else
            {
                tasks = await _taskService.GetAllTasksAsync();
            }

            return Ok(tasks.Select(TaskMapper.ToDto));
        }

        // Получить задачу по id
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();
            return Ok(TaskMapper.ToDto(task));
        }

        // Создать задачу (Head, Chairman)
        [HttpPost]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto taskDto)
        {
            try
            {
                var createdTask = await _taskService.CreateTaskAsync(TaskMapper.ToEntity(taskDto));
                return CreatedAtAction(nameof(GetTask), new { id = createdTask.TaskId }, TaskMapper.ToDto(createdTask));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Обновить задачу (Head, Chairman)
        [HttpPut("{id}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] UpdateTaskDto taskDto)
        {
            if (id != taskDto.TaskId)
                return BadRequest();

            var taskEntity = await _taskService.GetTaskByIdAsync(id);
            if (taskEntity == null)
                return NotFound();

            TaskMapper.UpdateEntity(taskEntity, taskDto);
            var updatedTask = await _taskService.UpdateTaskAsync(taskEntity);
            return Ok(TaskMapper.ToDto(updatedTask));
        }

        // Удалить задачу (Head, Chairman)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> DeleteTask(Guid id)
        {
            var result = await _taskService.DeleteTaskAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // Назначить пользователя на задачу (Head, Chairman)
        [HttpPost("{id}/assign")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> AssignTask(Guid id, [FromBody] AssignTaskRequestDto request)
        {
            var result = await _taskService.AssignTaskToUserAsync(id, request.UserId, request.Role);
            if (!result)
                return BadRequest();
            return NoContent();
        }

        // Снять пользователя с задачи (Head, Chairman)
        [HttpDelete("{id}/assign/{userId}")]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult> UnassignTask(Guid id, Guid userId)
        {
            var result = await _taskService.RemoveUserFromTaskAsync(id, userId);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }

    // DTO для назначения задачи
    public class AssignTaskRequestDto
    {
        public Guid UserId { get; set; }
        public TaskUserRole Role { get; set; }
    }
}