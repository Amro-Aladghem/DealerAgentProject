using Application.Dtos;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> CreateOrder(Guid storeId, OrderDto dto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                StoreId = storeId,
                CreatedAt = DateTime.UtcNow,
                Status = dto.Status,
                TotalAmount = dto.TotalAmount
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return ToDto(order);
        }

        public async Task<OrderDto?> GetOrderById(Guid storeId, Guid id)
        {
            return await _context.Orders
                .Where(o => o.Id == id && o.StoreId == storeId)
                .Select(o => ToDto(o))
                .FirstOrDefaultAsync();
        }

        public async Task<List<OrderDto>> GetOrdersForStore(Guid storeId)
        {
            return await _context.Orders
                .Where(o => o.StoreId == storeId)
                .OrderBy(o => o.CreatedAt)
                .Select(o => ToDto(o))
                .ToListAsync();
        }

        public async Task<List<OrderDto>> GetOrdersForSpecificStatus(Guid storeId, eOrderStatus status)
        {
            return await _context.Orders
                .Where(o => o.StoreId == storeId && o.Status == status)
                .OrderBy(o => o.CreatedAt)
                .Select(o => ToDto(o))
                .ToListAsync();
        }

        public async Task<OrderDto?> UpdateOrder(Guid storeId, Guid id, OrderDto dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.StoreId == storeId);

            if (order is null)
                return null;

            order.Status = dto.Status;
            order.TotalAmount = dto.TotalAmount;

            await _context.SaveChangesAsync();

            return ToDto(order);
        }

        public async Task<OrderDto?> UpdateOrderStatus(Guid storeId, Guid id, eOrderStatus status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.StoreId == storeId);

            if (order is null)
                return null;

            order.Status = status;

            await _context.SaveChangesAsync();

            return ToDto(order);
        }

        public async Task<bool> DeleteOrder(Guid storeId, Guid id)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.StoreId == storeId);

            if (order is null)
                return false;

            var rows = _context.OrderItems
                .Where(r => r.OrderId == id);

            _context.OrderItems.RemoveRange(rows);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        private static OrderDto ToDto(Order o)
        {
            return new OrderDto
            {
                Id = o.Id,
                StoreId = o.StoreId,
                CreatedAt = o.CreatedAt,
                Status = o.Status,
                TotalAmount = o.TotalAmount
            };
        }
    }
}
