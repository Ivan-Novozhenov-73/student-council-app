using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;
using System;

namespace StudentCouncilAPI.Mappers
{
    public static class LinkMappers
    {
        public static EventUser ToEntity(EventUserLinkDto dto)
        {
            if (dto == null) return null;
            return new EventUser
            {
                UserId = dto.UserId,
                EventId = dto.EventId,
                Role = dto.Role
            };
        }

        public static EventUserLinkDto ToDto(EventUser entity)
        {
            if (entity == null) return null;
            return new EventUserLinkDto
            {
                UserId = entity.UserId,
                EventId = entity.EventId,
                Role = entity.Role
            };
        }

        public static TaskUser ToEntity(TaskUserLinkDto dto)
        {
            if (dto == null) return null;
            return new TaskUser
            {
                UserId = dto.UserId,
                TaskId = dto.TaskId,
                Role = dto.Role
            };
        }

        public static TaskUserLinkDto ToDto(TaskUser entity)
        {
            if (entity == null) return null;
            return new TaskUserLinkDto
            {
                UserId = entity.UserId,
                TaskId = entity.TaskId,
                Role = entity.Role
            };
        }

        public static MeetingUser ToEntity(MeetingUserLinkDto dto)
        {
            if (dto == null) return null;
            return new MeetingUser
            {
                UserId = dto.UserId,
                MeetingId = dto.MeetingId,
                Role = dto.Role
            };
        }

        public static MeetingUserLinkDto ToDto(MeetingUser entity)
        {
            if (entity == null) return null;
            return new MeetingUserLinkDto
            {
                UserId = entity.UserId,
                MeetingId = entity.MeetingId,
                Role = entity.Role
            };
        }

        public static EventPartner ToEntity(EventPartnerLinkDto dto)
        {
            if (dto == null) return null;
            return new EventPartner
            {
                PartnerId = dto.PartnerId,
                EventId = dto.EventId
            };
        }

        public static EventPartnerLinkDto ToDto(EventPartner entity)
        {
            if (entity == null) return null;
            return new EventPartnerLinkDto
            {
                PartnerId = entity.PartnerId,
                EventId = entity.EventId
            };
        }
    }
}