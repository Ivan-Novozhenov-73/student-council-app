namespace StudentCouncilAPI.DTOs;

public class LoginRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginResponse
{
    public string Token { get; set; } = null!;
    public UserDto User { get; set; } = null!;
}

public class RegisterRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Patronymic { get; set; }
    public string Group { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Contacts { get; set; }
}

public class RegisterResponse
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = null!;
}