using Microsoft.EntityFrameworkCore;
using StudentCouncilAPI.Data;
using StudentCouncilAPI.Models;

namespace StudentCouncilAPI.Services
{
    public class PartnerService : IPartnerService
    {
        private readonly ApplicationDbContext _context;

        public PartnerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
        {
            return await _context.Partners
                .Where(p => !p.Archive)
                .OrderBy(p => p.Surname)
                .ToListAsync();
        }

        public async Task<Partner?> GetPartnerByIdAsync(Guid id)
        {
            return await _context.Partners
                .Include(p => p.EventPartners)
                .ThenInclude(ep => ep.Event)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.PartnerId == id);
        }

        public async Task<Partner> CreatePartnerAsync(Partner partner)
        {
            partner.PartnerId = Guid.NewGuid();
            _context.Partners.Add(partner);
            await _context.SaveChangesAsync();
            return partner;
        }

        public async Task<Partner> UpdatePartnerAsync(Partner partner)
        {
            _context.Partners.Update(partner);
            await _context.SaveChangesAsync();
            return partner;
        }

        public async Task<bool> DeletePartnerAsync(Guid id)
        {
            var partner = await GetPartnerByIdAsync(id);
            if (partner == null) return false;

            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ArchivePartnerAsync(Guid id)
        {
            var partner = await GetPartnerByIdAsync(id);
            if (partner == null) return false;

            partner.Archive = !partner.Archive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Partner>> SearchPartnersAsync(string searchTerm)
        {
            return await _context.Partners
                .Where(p => !p.Archive && (
                    p.Surname.Contains(searchTerm) ||
                    p.Name.Contains(searchTerm) ||
                    (p.Patronymic != null && p.Patronymic.Contains(searchTerm)) ||
                    p.Description.Contains(searchTerm)
                ))
                .OrderBy(p => p.Surname)
                .ToListAsync();
        }

        public async Task<IEnumerable<Partner>> GetPartnersByEventAsync(Guid eventId)
        {
            return await _context.Partners
                .Where(p => p.EventPartners.Any(ep => ep.EventId == eventId))
                .OrderBy(p => p.Surname)
                .ToListAsync();
        }
    }
}
