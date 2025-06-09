using Microsoft.EntityFrameworkCore;
using StudentCouncilAPI.Data;
using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly ApplicationDbContext _context;

        public MeetingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
        {
            return await _context.Meetings
                .Include(m => m.MeetingUsers)
                .ThenInclude(mu => mu.User)
                .OrderByDescending(m => m.MeetingDate)
                .ToListAsync();
        }

        public async Task<Meeting?> GetMeetingByIdAsync(Guid id)
        {
            return await _context.Meetings
                .Include(m => m.MeetingUsers)
                .ThenInclude(mu => mu.User)
                .FirstOrDefaultAsync(m => m.MeetingId == id);
        }

        public async Task<Meeting> CreateMeetingAsync(Meeting meeting)
        {
            meeting.MeetingId = Guid.NewGuid();
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }

        public async Task<Meeting> UpdateMeetingAsync(Meeting meeting)
        {
            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();
            return meeting;
        }

        public async Task<bool> DeleteMeetingAsync(Guid id)
        {
            var meeting = await GetMeetingByIdAsync(id);
            if (meeting == null) return false;

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Meetings
                .Where(m => m.MeetingDate >= startDate && m.MeetingDate <= endDate)
                .Include(m => m.MeetingUsers)
                .ThenInclude(mu => mu.User)
                .OrderBy(m => m.MeetingDate)
                .ThenBy(m => m.MeetingTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsByUserAsync(Guid userId)
        {
            return await _context.Meetings
                .Where(m => m.MeetingUsers.Any(mu => mu.UserId == userId))
                .Include(m => m.MeetingUsers)
                .ThenInclude(mu => mu.User)
                .OrderByDescending(m => m.MeetingDate)
                .ToListAsync();
        }

        public async Task<bool> AddUserToMeetingAsync(Guid meetingId, Guid userId, MeetingUserRole role)
        {
            var existingRelation = await _context.MeetingUsers
                .FirstOrDefaultAsync(mu => mu.MeetingId == meetingId && mu.UserId == userId);

            if (existingRelation != null)
            {
                existingRelation.Role = role;
            }
            else
            {
                _context.MeetingUsers.Add(new MeetingUser
                {
                    MeetingId = meetingId,
                    UserId = userId,
                    Role = role
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromMeetingAsync(Guid meetingId, Guid userId)
        {
            var relation = await _context.MeetingUsers
                .FirstOrDefaultAsync(mu => mu.MeetingId == meetingId && mu.UserId == userId);

            if (relation == null) return false;

            _context.MeetingUsers.Remove(relation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
