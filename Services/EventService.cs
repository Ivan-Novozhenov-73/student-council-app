using Microsoft.EntityFrameworkCore;
using StudentCouncilAPI.Data;
using StudentCouncilAPI.Models;
using StudentCouncilAPI.DTOs;

namespace StudentCouncilAPI.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            var events = await _context.Events
                .Include(e => e.EventUsers).ThenInclude(eu => eu.User)
                .Include(e => e.Tasks)
                .Include(e => e.Notes)
                .Include(e => e.EventPartners)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            return events.Select(MapToDto);
        }

        public async Task<EventDto?> GetEventByIdAsync(Guid id)
        {
            var e = await _context.Events
                .Include(ev => ev.EventUsers).ThenInclude(eu => eu.User)
                .Include(ev => ev.Tasks)
                .Include(ev => ev.Notes)
                .Include(ev => ev.EventPartners)
                .FirstOrDefaultAsync(ev => ev.EventId == id);

            return e == null ?  null : MapToDto(e);
        }

        public async Task<EventDto> CreateEventAsync(CreateEventDto request)
        {
            var e = new Event
            {
                EventId = Guid.NewGuid(),
                Title = request.Title,
                Status = request.Status,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                EventTime = request.EventTime,
                Location = request.Location,
                NumberOfParticipants = request.NumberOfParticipants
            };
            _context.Events.Add(e);
            await _context.SaveChangesAsync();
            return MapToDto(e);
        }

        public async Task<EventDto> UpdateEventAsync(Guid id, UpdateEventDto request)
        {
            var e = await _context.Events
                .Include(ev => ev.EventUsers).ThenInclude(eu => eu.User)
                .Include(ev => ev.Tasks)
                .Include(ev => ev.Notes)
                .Include(ev => ev.EventPartners)
                .FirstOrDefaultAsync(ev => ev.EventId == id);

            if (e == null) throw new Exception("Event not found");

            e.Title = request.Title;
            e.Status = request.Status;
            e.Description = request.Description;
            e.StartDate = request.StartDate;
            e.EndDate = request.EndDate;
            e.EventTime = request.EventTime;
            e.Location = request.Location;
            e.NumberOfParticipants = request.NumberOfParticipants;

            await _context.SaveChangesAsync();
            return MapToDto(e);
        }

        public async Task<bool> DeleteEventAsync(Guid id)
        {
            var eventItem = await _context.Events.FirstOrDefaultAsync(x => x.EventId == id);
            if (eventItem == null) return false;
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EventDto>> GetEventsByStatusAsync(EventStatus status)
        {
            var events = await _context.Events
                .Where(e => e.Status == status)
                .Include(e => e.EventUsers).ThenInclude(eu => eu.User)
                .Include(e => e.Tasks)
                .Include(e => e.Notes)
                .Include(e => e.EventPartners)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            return events.Select(MapToDto);
        }

        public async Task<IEnumerable<EventDto>> GetEventsByUserAsync(Guid userId)
        {
            var events = await _context.Events
                .Where(e => e.EventUsers.Any(eu => eu.UserId == userId))
                .Include(e => e.EventUsers).ThenInclude(eu => eu.User)
                .Include(e => e.Tasks)
                .Include(e => e.Notes)
                .Include(e => e.EventPartners)
                .OrderByDescending(e => e.StartDate)
                .ToListAsync();

            return events.Select(MapToDto);
        }

        public async Task<bool> AddUserToEventAsync(Guid eventId, Guid userId, EventUserRole role)
        {
            var existingRelation = await _context.EventUsers
                .FirstOrDefaultAsync(eu => eu.EventId == eventId && eu.UserId == userId);

            if (existingRelation != null)
            {
                existingRelation.Role = role;
            }
            else
            {
                _context.EventUsers.Add(new EventUser
                {
                    EventId = eventId,
                    UserId = userId,
                    Role = role
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromEventAsync(Guid eventId, Guid userId)
        {
            var relation = await _context.EventUsers
                .FirstOrDefaultAsync(eu => eu.EventId == eventId && eu.UserId == userId);

            if (relation == null) return false;

            _context.EventUsers.Remove(relation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EventUserDto>> GetEventParticipantsAsync(Guid eventId)
        {
            var participants = await _context.EventUsers
                .Where(eu => eu.EventId == eventId)
                .Include(eu => eu.User)
                .ToListAsync();

            return participants.Select(eu => new EventUserDto
            {
                UserId = eu.UserId,
                FullName = $"{eu.User.Surname} {eu.User.Name} {eu.User.Patronymic}".Trim(),
                Role = eu.Role
            });
        }

        private static EventDto MapToDto(Event e) =>
            new EventDto
            {
                EventId = e.EventId,
                Title = e.Title,
                Status = e.Status,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                EventTime = e.EventTime,
                Location = e.Location,
                NumberOfParticipants = e.NumberOfParticipants,
                Participants = e.EventUsers.Select(eu => new EventUserDto
                {
                    UserId = eu.UserId,
                    FullName = $"{eu.User.Surname} {eu.User.Name} {eu.User.Patronymic}".Trim(),
                    Role = eu.Role
                }).ToList(),
                Tasks = e.Tasks.Select(t => new TaskDto
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted
                }).ToList(),
                Notes = e.Notes.Select(n => new NoteDto
                {
                    NoteId = n.NoteId,
                    Content = n.Content
                }).ToList(),
                Partners = e.EventPartners.Select(p => new EventPartnerDto
                {
                    PartnerId = p.PartnerId,
                    Name = p.Name
                }).ToList()
            };
    }
}
