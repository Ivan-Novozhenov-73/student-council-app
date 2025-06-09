using Microsoft.AspNetCore.Mvc;
using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Mappers;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentCouncilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, IUserService userService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginDto request)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Login}", request.Login);

                var user = await _userService.GetUserByLoginAsync(request.Login);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Login}", request.Login);
                    return Unauthorized(new { message = "Неверный логин или пароль" });
                }

                if (!_authService.VerifyPassword(request.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Invalid password for user: {Login}", request.Login);
                    return Unauthorized(new { message = "Неверный логин или пароль" });
                }

                var token = _authService.GenerateJwtToken(user);

                var userDto = UserMapper.ToDto(user);

                _logger.LogInformation("Login successful for user: {Login}", request.Login);
                return Ok(new AuthResultDto
                {
                    Token = token,
                    User = userDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login: {Message}", ex.Message);
                return StatusCode(500, new { message = "Внутренняя ошибка при входе" });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] CreateUserDto request)
        {
            try
            {
                var user = new User
                {
                    Login = request.Login,
                    Surname = request.Surname,
                    Name = request.Name,
                    Patronymic = request.Patronymic,
                    Group = request.Group,
                    Phone = request.Phone,
                    Contacts = request.Contacts,
                    // Присваиваем роль по умолчанию, например, активист
                    Role = UserRole.Activist
                };

                var createdUser = await _authService.RegisterAsync(user, request.Password);
                return Ok(new { message = "Регистрация прошла успешно", userId = createdUser.UserId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("verify-token")]
        public ActionResult VerifyToken()
        {
            try
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { message = "Необходима авторизация" });
                }

                var token = authHeader.Substring("Bearer ".Length);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var login = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(login))
                {
                    return Unauthorized(new { message = "Невалидный токен" });
                }

                return Ok(new
                {
                    message = "Токен действителен",
                    userId,
                    login,
                    role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying token: {Message}", ex.Message);
                return Unauthorized(new { message = "Невалидный токен" });
            }
        }

        // DTO для Login и регистрации используются из StudentCouncilAPI.DTOs
        // AuthResultDto также из StudentCouncilAPI.DTOs
    }
}