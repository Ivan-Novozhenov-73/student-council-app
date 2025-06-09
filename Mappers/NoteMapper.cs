using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;
using System;

namespace StudentCouncilAPI.Mappers
{
    public static class NoteMapper
    {
        public static NoteDto ToDto(Note entity)
        {
            if (entity == null) return null;
            return new NoteDto
            {
                NoteId = entity.NoteId,
                UserId = entity.UserId,
                EventId = entity.EventId,
                Title = entity.Title,
                FilePath = entity.FilePath
            };
        }

        public static Note ToEntity(CreateNoteDto dto)
        {
            if (dto == null) return null;
            return new Note
            {
                NoteId = Guid.NewGuid(),
                UserId = dto.UserId,
                EventId = dto.EventId,
                Title = dto.Title,
                FilePath = dto.FilePath
            };
        }

        public static void UpdateEntity(Note entity, UpdateNoteDto dto)
        {
            if (dto == null || entity == null) return;
            if (dto.Title != null) entity.Title = dto.Title;
            if (dto.FilePath != null) entity.FilePath = dto.FilePath;
        }
    }
}