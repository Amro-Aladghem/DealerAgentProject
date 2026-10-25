using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CarAccessoryService
    {
        private readonly AppDbContext _context;

        public CarAccessoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarAccessoryDto> AddCarAccessory(Guid storeId, CarAccessoryDto dto)
        {
            var carAccessory = new CarAccessory
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                ImageUrl = dto.ImageUrl,
                ProductUrl = dto.ProductUrl
            };

            _context.CarAccessories.Add(carAccessory);
            await _context.SaveChangesAsync();

            return ToDto(carAccessory);
        }

        public async Task<List<CarAccessoryDto>> AddCarAccessories(Guid storeId, List<CarAccessoryDto> dtos)
        {
            var carAccessories = dtos.Select(dto => new CarAccessory
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                ImageUrl = dto.ImageUrl,
                ProductUrl = dto.ProductUrl
            }).ToList();

            await _context.CarAccessories.AddRangeAsync(carAccessories);
            await _context.SaveChangesAsync();

            return carAccessories.Select(ToDto).ToList();
        }

        public async Task<CarAccessoryDto?> UpdateCarAccessoryById(Guid storeId, Guid id, CarAccessoryDto dto)
        {
            var carAccessory = await _context.CarAccessories
                .FirstOrDefaultAsync(a => a.Id == id && a.StoreId == storeId);

            if (carAccessory is null)
                return null;

            carAccessory.Name = dto.Name;
            carAccessory.Description = dto.Description;
            carAccessory.Price = dto.Price;
            carAccessory.Quantity = dto.Quantity;
            carAccessory.ImageUrl = dto.ImageUrl;
            carAccessory.ProductUrl = dto.ProductUrl;

            await _context.SaveChangesAsync();

            return ToDto(carAccessory);
        }

        public async Task<CarAccessoryDto?> GetCarAccessoryById(Guid storeId, Guid id)
        {
            return await _context.CarAccessories
                .Where(a => a.Id == id && a.StoreId == storeId)
                .Select(a => ToDto(a))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteCarAccessory(Guid storeId, Guid id)
        {
            var carAccessory = await _context.CarAccessories
                .FirstOrDefaultAsync(a => a.Id == id && a.StoreId == storeId);

            if (carAccessory is null)
                return false;

            _context.CarAccessories.Remove(carAccessory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CarAccessoryDto>> GetCarAccessories(Guid storeId)
        {
            return await _context.CarAccessories
                .Where(a => a.StoreId == storeId)
                .Select(a => ToDto(a))
                .ToListAsync();
        }

        public async Task<List<CarAccessoryDto>> GetCarAccessoriesByName(Guid storeId, string name)
        {
            return await _context.CarAccessories
                .Where(a => a.StoreId == storeId && a.Name.Contains(name))
                .Select(a => ToDto(a))
                .ToListAsync();
        }

        public async Task<CarAccessoryDto?> GetCarAccessoryByName(Guid storeId, string name)
        {
            return await _context.CarAccessories
                .Where(a => a.StoreId == storeId && a.Name == name)
                .Select(a => ToDto(a))
                .FirstOrDefaultAsync();
        }

        private static CarAccessoryDto ToDto(CarAccessory a)
        {
            return new CarAccessoryDto
            {
                Id = a.Id,
                StoreId = a.StoreId,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                Quantity = a.Quantity,
                ImageUrl = a.ImageUrl,
                ProductUrl = a.ProductUrl
            };
        }
    }
}
