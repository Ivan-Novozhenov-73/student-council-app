using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;
using System;

namespace StudentCouncilAPI.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            if (user == null) return null;
            return new UserDto
            {
                UserId = user.UserId,
                Login = user.Login,
                Surname = user.Surname,
                Name = user.Name,
                Patronymic = user.Patronymic,
                Role = (int)user.Role,
                Group = user.Group,
                Phone = user.Phone,
                Contacts = user.Contacts,
                Archive = user.Archive
            };
        }

        public static User ToEntity(CreateUserDto dto, string passwordHash)
        {
            if (dto == null) return null;
            return new User
            {
                UserId = Guid.NewGuid(),
                Login = dto.Login,
                PasswordHash = passwordHash,
                Surname = dto.Surname,
                Name = dto.Name,
                Patronymic = dto.Patronymic,
                Role = (UserRole)dto.Role,
                Group = dto.Group,
                Phone = dto.Phone,
                Contacts = dto.Contacts,
                Archive = false
            };
        }

        public static void UpdateEntity(User user, UpdateUserDto dto)
        {
            if (dto == null || user == null) return;
            if (dto.Surname != null) user.Surname = dto.Surname;
            if (dto.Name != null) user.Name = dto.Name;
            if (dto.Patronymic != null) user.Patronymic = dto.Patronymic;
            if (dto.Role.HasValue) user.Role = (UserRole)dto.Role.Value;
            if (dto.Group != null) user.Group = dto.Group;
            if (dto.Phone.HasValue) user.Phone = dto.Phone.Value;
            if (dto.Contacts != null) user.Contacts = dto.Contacts;
            if (dto.Archive.HasValue) user.Archive = dto.Archive.Value;
        }
    }
}