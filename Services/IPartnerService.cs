using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public interface IPartnerService
    {
        Task<IEnumerable<Partner>> GetAllPartnersAsync();
        Task<Partner?> GetPartnerByIdAsync(Guid id);
        Task<Partner> CreatePartnerAsync(Partner partner);
        Task<Partner> UpdatePartnerAsync(Partner partner);
        Task<bool> DeletePartnerAsync(Guid id);
        Task<bool> ArchivePartnerAsync(Guid id);
        Task<IEnumerable<Partner>> SearchPartnersAsync(string searchTerm);
        Task<IEnumerable<Partner>> GetPartnersByEventAsync(Guid eventId);
    }
}
