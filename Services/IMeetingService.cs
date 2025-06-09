using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public interface IMeetingService
    {
        Task<IEnumerable<Meeting>> GetAllMeetingsAsync();
        Task<Meeting?> GetMeetingByIdAsync(Guid id);
        Task<Meeting> CreateMeetingAsync(Meeting meeting);
        Task<Meeting> UpdateMeetingAsync(Meeting meeting);
        Task<bool> DeleteMeetingAsync(Guid id);
        Task<IEnumerable<Meeting>> GetMeetingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Meeting>> GetMeetingsByUserAsync(Guid userId);
        Task<bool> AddUserToMeetingAsync(Guid meetingId, Guid userId, MeetingUserRole role);
        Task<bool> RemoveUserFromMeetingAsync(Guid meetingId, Guid userId);
    }
}
