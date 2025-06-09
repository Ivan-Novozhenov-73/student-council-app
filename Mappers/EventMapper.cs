using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;
using System;

namespace StudentCouncilAPI.Mappers
{
    public static class EventMapper
    {
        public static EventDto ToDto(Event entity)
        {
            if (entity == null) return null;
            return new EventDto
            {
                EventId = entity.EventId,
                Title = entity.Title,
                Status = (int)entity.Status,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                EventTime = entity.EventTime,
                Location = entity.Location,
                NumberOfParticipants = entity.NumberOfParticipants
            };
        }

        public static Event ToEntity(CreateEventDto dto)
        {
            if (dto == null) return null;
            return new Event
            {
                EventId = Guid.NewGuid(),
                Title = dto.Title,
                Status = (EventStatus)dto.Status,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                EventTime = dto.EventTime,
                Location = dto.Location,
                NumberOfParticipants = dto.NumberOfParticipants
            };
        }

        public static void UpdateEntity(Event entity, UpdateEventDto dto)
        {
            if (dto == null || entity == null) return;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.Status.HasValue) entity.Status = (EventStatus)dto.Status.Value;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.StartDate.HasValue) entity.StartDate = dto.StartDate;
            if (dto.EndDate.HasValue) entity.EndDate = dto.EndDate;
            if (dto.EventTime.HasValue) entity.EventTime = dto.EventTime;
            if (dto.Location != null) entity.Location = dto.Location;
            if (dto.NumberOfParticipants.HasValue) entity.NumberOfParticipants = dto.NumberOfParticipants;
        }
    }
}