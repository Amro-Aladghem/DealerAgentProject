using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class StoreService
    {
        private readonly AppDbContext _context;

        public StoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StoreDto> CreateStore(StoreDto dto)
        {
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ProductsUrl = dto.ProductsUrl,
                ComplaintsFileUrl = dto.ComplaintsFileUrl
            };

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return ToDto(store);
        }

        public async Task<StoreDto?> GetStoreById(Guid id)
        {
            return await _context.Stores
                .Where(s => s.Id == id)
                .Select(s => ToDto(s))
                .FirstOrDefaultAsync();
        }

        public async Task<StoreDto?> GetStoreByName(string name)
        {
            return await _context.Stores
                .Where(s => s.Name == name)
                .Select(s => ToDto(s))
                .FirstOrDefaultAsync();
        }

        public async Task<List<StoreDto>> GetStores()
        {
            return await _context.Stores
                .Select(s => ToDto(s))
                .ToListAsync();
        }

        public async Task<StoreDto?> UpdateStore(Guid id, StoreDto dto)
        {
            var store = await _context.Stores
                .FirstOrDefaultAsync(s => s.Id == id);

            if (store is null)
                return null;

            store.Name = dto.Name;
            store.ProductsUrl = dto.ProductsUrl;
            store.ComplaintsFileUrl = dto.ComplaintsFileUrl;

            await _context.SaveChangesAsync();

            return ToDto(store);
        }

        private static StoreDto ToDto(Store s)
        {
            return new StoreDto
            {
                Id = s.Id,
                Name = s.Name,
                ProductsUrl = s.ProductsUrl,
                ComplaintsFileUrl = s.ComplaintsFileUrl
            };
        }
    }
}
