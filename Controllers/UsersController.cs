using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Mappers;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentCouncilAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        // Получить всех пользователей (с фильтрацией по роли, группе, поиску)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            [FromQuery] UserRole? role = null,
            [FromQuery] string? group = null,
            [FromQuery] string? search = null)
        {
            IEnumerable<User> users;
            if (!string.IsNullOrEmpty(search))
            {
                users = await _userService.SearchUsersAsync(search);
            }
            else if (role.HasValue)
            {
                users = await _userService.GetUsersByRoleAsync(role.Value);
            }
            else if (!string.IsNullOrEmpty(group))
            {
                users = await _userService.GetUsersByGroupAsync(group);
            }
            else
            {
                users = await _userService.GetAllUsersAsync();
            }
            return Ok(users.Select(UserMapper.ToDto));
        }

        // Получить пользователя по id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(UserMapper.ToDto(user));
        }

        // Получить пользователя по логину (для AuthController, внутренний вызов)
        [HttpGet("by-login/{login}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> GetUserByLogin(string login)
        {
            var user = await _userService.GetUserByLoginAsync(login);
            if (user == null)
                return NotFound();
            return Ok(UserMapper.ToDto(user));
        }

        // Создать пользователя (только глава/председатель)
        [HttpPost]
        [Authorize(Roles = "Head,Chairman")]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto userDto)
        {
            var passwordHash = _authService.HashPassword(userDto.Password);
            var user = UserMapper.ToEntity(userDto, passwordHash);
            var created = await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = created.UserId }, UserMapper.ToDto(created));
        }

        // Обновить пользователя (Активист - только свой профиль; Глава/Председатель - любой)
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserDto updateDto)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Если не глава/председатель, можно редактировать только свой профиль
            if (currentUserRole != "Chairman" && currentUserRole != "Head" && currentUserId != user.UserId.ToString())
                return Forbid();

            UserMapper.UpdateEntity(user, updateDto);
            var updated = await _userService.UpdateUserAsync(user);
            return Ok(UserMapper.ToDto(updated));
        }

        // Удалить пользователя (только председатель)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Chairman")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // Архивировать/разархивировать пользователя (только председатель)
        [HttpPatch("{id}/archive")]
        [Authorize(Roles = "Chairman")]
        public async Task<ActionResult> ArchiveUser(Guid id)
        {
            var result = await _userService.ArchiveUserAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}