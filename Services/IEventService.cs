using StudentCouncilAPI.DTOs;
using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEventsAsync();
        Task<EventDto?> GetEventByIdAsync(Guid id);
        Task<EventDto> CreateEventAsync(CreateEventDto request);
        Task<EventDto> UpdateEventAsync(Guid id, UpdateEventDto request);
        Task<bool> DeleteEventAsync(Guid id);
        Task<IEnumerable<EventDto>> GetEventsByStatusAsync(EventStatus status);
        Task<IEnumerable<EventDto>> GetEventsByUserAsync(Guid userId);
        Task<bool> AddUserToEventAsync(Guid eventId, Guid userId, EventUserRole role);
        Task<bool> RemoveUserFromEventAsync(Guid eventId, Guid userId);
        Task<IEnumerable<EventUserDto>> GetEventParticipantsAsync(Guid eventId);
    }
}