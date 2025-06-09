using Microsoft.IdentityModel.Tokens;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudentCouncilAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserService userService, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Login}", request.Login);
                
                var userEntity = await _userService.GetUserByLoginAsync(request.Login);
                
                if (userEntity == null || !VerifyPassword(request.Password, userEntity.PasswordHash))
                {
                    _logger.LogWarning("Login failed: user not found or invalid password");
                    throw new UnauthorizedAccessException("Неверный логин или пароль");
                }

                if (userEntity.Archive)
                {
                    _logger.LogWarning("Login failed: user archived");
                    throw new UnauthorizedAccessException("Аккаунт заблокирован");
                }

                var token = GenerateJwtToken(userEntity);
                _logger.LogInformation("Login successful for user: {Login}", request.Login);

                var userDto = _userService.MapToUserDto(userEntity);

                return new LoginResponse
                {
                    Token = token,
                    User = userDto
                };
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Пробрасываем дальше
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user {Login}: {Message}", login, ex.Message);
                throw new InvalidOperationException($"Ошибка входа: {ex.Message}", ex);
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Registration attempt for user: {Login}", request.Login);

                var existing = await _userService.GetUserByLoginAsync(request.Login);
                if (existing != null)
                {
                    _logger.LogWarning("Registration failed: user exists");
                    throw new InvalidOperationException("Пользователь с таким логином уже существует");
                }

                var entity = new User
                {
                    UserId = Guid.NewGuid(),
                    Login = request.Login,
                    Surname = request.Surname,
                    Name = request.Name,
                    Patronymic = request.Patronymic,
                    Group = request.Group,
                    Phone = request.Phone,
                    Contacts = request.Contacts,
                    PasswordHash = HashPassword(request.Password),
                    Role = UserRole.Activist
                };
                
                await _userService.CreateUserAsync(entity);

                return new RegisterResponse
                {
                    UserID = entity.UserId,
                    Message = "Пользователь успешно зарегистрирован"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration error for user {Login}: {Message}", request.Login, ex.Message);
                throw;
            }
        }

        public string GenerateJwtToken(User user)
        {
            try
            {
                _logger.LogInformation("Generating JWT token for user: {Login}", user.Login);
                
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                
                if (string.IsNullOrEmpty(secretKey))
                {
                    _logger.LogError("JWT SecretKey is missing in configuration");
                    throw new InvalidOperationException("JWT SecretKey отсутствует в конфигурации");
                }
                
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                
                if (keyBytes.Length < 32)
                {
                    _logger.LogError("JWT SecretKey is too short: {Length} bytes", keyBytes.Length);
                    throw new InvalidOperationException("JWT SecretKey слишком короткий. Должен быть не менее 32 символов");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim("FullName", $"{user.Surname} {user.Name} {user.Patronymic}".Trim()),
                    new Claim("Group", user.Group)
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(keyBytes),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                
                _logger.LogInformation("JWT token generated successfully for user: {Login}", user.Login);
                
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token: {Message}", ex.Message);
                throw new InvalidOperationException($"Ошибка генерации JWT токена: {ex.Message}", ex);
            }
        }

        public string HashPassword(string password)
        {
            _logger.LogInformation("Hashing password");
            
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashedBytes = sha256.ComputeHash(passwordBytes);
            var hash = Convert.ToBase64String(hashedBytes);
            
            _logger.LogInformation("Password hashed successfully");
            return hash;
        }

        public bool VerifyPassword(string password, string hash)
        {
            _logger.LogInformation("Verifying password");
            
            var hashedPassword = HashPassword(password);
            var result = hashedPassword == hash;
            
            _logger.LogInformation("Password verification result: {Result}", result);
            
            return result;
        }
    }
}
