using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CarService
    {
        private readonly AppDbContext _context;

        public CarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarDto> AddCar(Guid storeId, CarDto dto)
        {
            var car = new Car
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                CarMakeId = dto.CarMakeId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                ModelYear = dto.ModelYear,
                ImageUrl = dto.ImageUrl,
                ProductUrl = dto.ProductUrl
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return ToDto(car);
        }

        public async Task<List<CarDto>> AddCars(Guid storeId, List<CarDto> dtos)
        {
            var cars = dtos.Select(dto => new Car
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                CarMakeId = dto.CarMakeId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                ModelYear = dto.ModelYear,
                ImageUrl = dto.ImageUrl,
                ProductUrl = dto.ProductUrl
            }).ToList();

            await _context.Cars.AddRangeAsync(cars);
            await _context.SaveChangesAsync();

            return cars.Select(ToDto).ToList();
        }

        public async Task<CarDto?> UpdateCarById(Guid storeId, Guid id, CarDto dto)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (car is null)
                return null;

            car.CarMakeId = dto.CarMakeId;
            car.Name = dto.Name;
            car.Description = dto.Description;
            car.Price = dto.Price;
            car.Quantity = dto.Quantity;
            car.ModelYear = dto.ModelYear;
            car.ImageUrl = dto.ImageUrl;
            car.ProductUrl = dto.ProductUrl;

            await _context.SaveChangesAsync();

            return ToDto(car);
        }

        public async Task<CarDto?> GetCarById(Guid storeId, Guid id)
        {
            return await _context.Cars
                .Where(c => c.Id == id && c.StoreId == storeId)
                .Select(c => ToDto(c))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteCar(Guid storeId, Guid id)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (car is null)
                return false;

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CarDto>> GetCars(Guid storeId)
        {
            return await _context.Cars
                .Where(c => c.StoreId == storeId)
                .Select(c => ToDto(c))
                .ToListAsync();
        }

        public async Task<List<CarDto>> GetCarsByName(Guid storeId, string name)
        {
            return await _context.Cars
                .Where(c => c.StoreId == storeId && c.Name.Contains(name))
                .Select(c => ToDto(c))
                .ToListAsync();
        }

        public async Task<CarDto?> GetCarByName(Guid storeId, string name)
        {
            return await _context.Cars
                .Where(c => c.StoreId == storeId && c.Name == name)
                .Select(c => ToDto(c))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CarDto>> GetCarsByCarMake(Guid storeId, int carMakeId)
        {
            return await _context.Cars
                .Where(c => c.StoreId == storeId && c.CarMakeId == carMakeId)
                .Select(c => ToDto(c))
                .ToListAsync();
        }

        private static CarDto ToDto(Car c)
        {
            return new CarDto
            {
                Id = c.Id,
                StoreId = c.StoreId,
                CarMakeId = c.CarMakeId,
                Name = c.Name,
                Description = c.Description,
                Price = c.Price,
                Quantity = c.Quantity,
                ModelYear = c.ModelYear,
                ImageUrl = c.ImageUrl,
                ProductUrl = c.ProductUrl
            };
        }
    }
}
