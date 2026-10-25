using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ComplaintService
    {
        private readonly AppDbContext _context;

        public ComplaintService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ComplaintDto> CreateComplaint(Guid storeId, ComplaintDto dto)
        {
            var complaint = new Complaint
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                Subject = dto.Subject,
                Description = dto.Description,
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                CreatedAt = DateTime.UtcNow
            };

            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

            return ToDto(complaint);
        }

        public async Task<ComplaintDto?> GetComplaintById(Guid storeId, Guid id)
        {
            return await _context.Complaints
                .Where(c => c.Id == id && c.StoreId == storeId)
                .Select(c => ToDto(c))
                .FirstOrDefaultAsync();
        }

        public async Task<List<ComplaintDto>> GetComplaintsForStore(Guid storeId)
        {
            return await _context.Complaints
                .Where(c => c.StoreId == storeId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => ToDto(c))
                .ToListAsync();
        }

        public async Task<List<ComplaintDto>> GetComplaintsForCustomerEmail(Guid storeId, string customerEmail)
        {
            return await _context.Complaints
                .Where(c => c.StoreId == storeId && c.CustomerEmail == customerEmail)
                .OrderBy(c => c.CreatedAt)
                .Select(c => ToDto(c))
                .ToListAsync();
        }

        public async Task<ComplaintDto?> UpdateComplaint(Guid storeId, Guid id, ComplaintDto dto)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (complaint is null)
                return null;

            complaint.Subject = dto.Subject;
            complaint.Description = dto.Description;
            complaint.CustomerName = dto.CustomerName;
            complaint.CustomerEmail = dto.CustomerEmail;

            await _context.SaveChangesAsync();

            return ToDto(complaint);
        }

        public async Task<bool> DeleteComplaint(Guid storeId, Guid id)
        {
            var complaint = await _context.Complaints
                .FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (complaint is null)
                return false;

            _context.Complaints.Remove(complaint);
            await _context.SaveChangesAsync();

            return true;
        }

        private static ComplaintDto ToDto(Complaint c)
        {
            return new ComplaintDto
            {
                Id = c.Id,
                StoreId = c.StoreId,
                Subject = c.Subject,
                Description = c.Description,
                CustomerName = c.CustomerName,
                CustomerEmail = c.CustomerEmail,
                CreatedAt = c.CreatedAt
            };
        }
    }
}
