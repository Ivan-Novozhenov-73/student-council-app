using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;
using System;

namespace StudentCouncilAPI.Mappers
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(Task entity)
        {
            if (entity == null) return null;
            return new TaskDto
            {
                TaskId = entity.TaskId,
                EventId = entity.EventId,
                PartnerId = entity.PartnerId,
                Title = entity.Title,
                Status = (int)entity.Status,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate
            };
        }

        public static Task ToEntity(CreateTaskDto dto)
        {
            if (dto == null) return null;
            return new Task
            {
                TaskId = Guid.NewGuid(),
                EventId = dto.EventId,
                PartnerId = dto.PartnerId,
                Title = dto.Title,
                Status = (TaskStatus)dto.Status,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }

        public static void UpdateEntity(Task entity, UpdateTaskDto dto)
        {
            if (dto == null || entity == null) return;
            if (dto.PartnerId.HasValue) entity.PartnerId = dto.PartnerId;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Status.HasValue) entity.Status = (TaskStatus)dto.Status.Value;
            if (dto.StartDate.HasValue) entity.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue) entity.EndDate = dto.EndDate.Value;
        }
    }
}