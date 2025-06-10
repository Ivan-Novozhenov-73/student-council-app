using System;

namespace StudentCouncilAPI.DTOs
{
    public class UserDto
    {
        public required Guid UserId { get; set; }
        public required string Login { get; set; }
        public required string Surname { get; set; }
        public required string Name { get; set; }
        public string? Patronymic { get; set; }
        public required int Role { get; set; }
        public required string Group { get; set; }
        public required long Phone { get; set; }
        public required string Contacts { get; set; }
        public required bool Archive { get; set; }
    }

    public class CreateUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public int Role { get; set; }
        public string Group { get; set; }
        public long Phone { get; set; }
        public string Contacts { get; set; }
    }

    public class UpdateUserDto
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }
        public int? Role { get; set; }
        public string? Group { get; set; }
        public long? Phone { get; set; }
        public string? Contacts { get; set; }
        public bool? Archive { get; set; }
    }

    public class LoginDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class AuthResultDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}