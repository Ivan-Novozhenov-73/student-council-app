using StudentCouncilAPI.DTOs;

namespace StudentCouncilAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<UserDto?> GetUserByLoginAsync(string login);
        Task<UserDto> CreateUserAsync(CreateUserDto request);
        Task<UserDto?> UpdateUserAsync(Guid userId, UpdateUserDto request);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> ArchiveUserAsync(Guid userId);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(string role);
        Task<IEnumerable<UserDto>> GetUsersByGroupAsync(string group);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm);
    }
}
