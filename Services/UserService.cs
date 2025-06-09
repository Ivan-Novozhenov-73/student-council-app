using Microsoft.EntityFrameworkCore;
using StudentCouncilAPI.Data;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.DTOs;

namespace StudentCouncilAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => !u.Archive)
                .OrderBy(u => u.Surname).ThenBy(u => u.Name)
                .Select(MapToDto)
                .ToListAsync();
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto?> GetUserByLoginAsync(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto request)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Login = request.Login,
                Surname = request.Surname,
                Name = request.Name,
                Patronymic = request.Patronymic,
                Group = request.Group,
                Phone = request.Phone,
                Contacts = request.Contacts,
                Role = request.Role,
                Archive = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return MapToDto(user);
        }

        public async Task<UserDto?> UpdateUserAsync(Guid userId, UpdateUserDto request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            user.Surname = request.Surname;
            user.Name = request.Name;
            user.Patronymic = request.Patronymic;
            user.Group = request.Group;
            user.Phone = request.Phone;
            user.Contacts = request.Contacts;
            user.Role = request.Role;
            user.Archive = request.Archive;

            await _context.SaveChangesAsync();
            return MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);;
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchiveUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Archive = !user.Archive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users
                .Where(u => u.Role == role && !u.Archive)
                .OrderBy(u => u.Surname)
                .ThenBy(u => u.Name)
                .Select(MapToDto)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetUsersByGroupAsync(string group)
        {
            return await _context.Users
                .Where(u => u.Group.Contains(group) && !u.Archive)
                .OrderBy(u => u.Surname)
                .ThenBy(u => u.Name)
                .Select(MapToDto)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm)
        {
            return await _context.Users
                .Where(u => 
                    (!u.Archive) && 
                    (u.Surname.Contains(searchTerm) ||
                    u.Name.Contains(searchTerm) ||
                    u.Login.Contains(searchTerm) ||
                    u.Group.Contains(searchTerm) ||
                    (u.Patronymic != null && u.Patronymic.Contains(searchTerm))))
                .OrderBy(u => u.Surname)
                .ThenBy(u => u.Name)
                .Select(MapToDto)
                .ToListAsync();
        }

        private static UserDto MapToDto(User user) => new UserDto
        {
            UserId = user.UserId,
            Login = user.Login,
            Surname = user.Surname,
            Name = user.Name,
            Patronymic = user.Patronymic,
            Group = user.Group,
            Phone = user.Phone,
            Contacts = user.Contacts,
            Role = user.Role,
            Archive = user.Archive
        };
    }
}