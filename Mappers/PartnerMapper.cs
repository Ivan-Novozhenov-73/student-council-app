using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;
using System;

namespace StudentCouncilAPI.Mappers
{
    public static class PartnerMapper
    {
        public static PartnerDto ToDto(Partner entity)
        {
            if (entity == null) return null;
            return new PartnerDto
            {
                PartnerId = entity.PartnerId,
                Surname = entity.Surname,
                Name = entity.Name,
                Patronymic = entity.Patronymic,
                Description = entity.Description,
                Phone = entity.Phone,
                Contacts = entity.Contacts,
                Archive = entity.Archive
            };
        }

        public static Partner ToEntity(CreatePartnerDto dto)
        {
            if (dto == null) return null;
            return new Partner
            {
                PartnerId = Guid.NewGuid(),
                Surname = dto.Surname,
                Name = dto.Name,
                Patronymic = dto.Patronymic,
                Description = dto.Description,
                Phone = dto.Phone,
                Contacts = dto.Contacts,
                Archive = false
            };
        }

        public static void UpdateEntity(Partner entity, UpdatePartnerDto dto)
        {
            if (dto == null || entity == null) return;
            if (dto.Surname != null) entity.Surname = dto.Surname;
            if (dto.Name != null) entity.Name = dto.Name;
            if (dto.Patronymic != null) entity.Patronymic = dto.Patronymic;
            if (dto.Description != null) entity.Description = dto.Description;
            if (dto.Phone.HasValue) entity.Phone = dto.Phone.Value;
            if (dto.Contacts != null) entity.Contacts = dto.Contacts;
            if (dto.Archive.HasValue) entity.Archive = dto.Archive.Value;
        }
    }
}