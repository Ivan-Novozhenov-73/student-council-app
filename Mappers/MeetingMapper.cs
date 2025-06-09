using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;
using System;

namespace StudentCouncilAPI.Mappers
{
    public static class MeetingMapper
    {
        public static MeetingDto ToDto(Meeting entity)
        {
            if (entity == null) return null;
            return new MeetingDto
            {
                MeetingId = entity.MeetingId,
                Title = entity.Title,
                MeetingDate = entity.MeetingDate,
                MeetingTime = entity.MeetingTime,
                Location = entity.Location,
                Link = entity.Link
            };
        }

        public static Meeting ToEntity(CreateMeetingDto dto)
        {
            if (dto == null) return null;
            return new Meeting
            {
                MeetingId = Guid.NewGuid(),
                Title = dto.Title,
                MeetingDate = dto.MeetingDate,
                MeetingTime = dto.MeetingTime,
                Location = dto.Location,
                Link = dto.Link
            };
        }

        public static void UpdateEntity(Meeting entity, UpdateMeetingDto dto)
        {
            if (dto == null || entity == null) return;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.MeetingDate.HasValue) entity.MeetingDate = dto.MeetingDate.Value;
            if (dto.MeetingTime.HasValue) entity.MeetingTime = dto.MeetingTime.Value;
            if (dto.Location != null) entity.Location = dto.Location;
            if (dto.Link != null) entity.Link = dto.Link;
        }
    }
}