using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class OrderItemService
    {
        private readonly AppDbContext _context;

        public OrderItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItemDto>> GetOrderItemsForOrder(Guid orderId, Guid storeId)
        {
            return await _context.OrderItems
                .Where(r => r.OrderId == orderId && r.Order.StoreId == storeId)
                .Select(r => ToDto(r))
                .ToListAsync();
        }

        public async Task<OrderItemDto?> GetOrderItemById(Guid orderId, Guid storeId, Guid id)
        {
            return await _context.OrderItems
                .Where(r => r.Id == id && r.OrderId == orderId && r.Order.StoreId == storeId)
                .Select(r => ToDto(r))
                .FirstOrDefaultAsync();
        }

        public async Task<OrderItemDto?> AddOrderItem(Guid orderId, Guid storeId, OrderItemDto dto)
        {
            bool orderExists = await _context.Orders
                .AnyAsync(o => o.Id == orderId && o.StoreId == storeId);

            if (!orderExists)
                return null;

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = dto.ProductId,
                ItemType = dto.ItemType,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return ToDto(orderItem);
        }

        public async Task<List<OrderItemDto>?> AddOrderItems(Guid orderId, Guid storeId, List<OrderItemDto> dtos)
        {
            bool orderExists = await _context.Orders
                .AnyAsync(o => o.Id == orderId && o.StoreId == storeId);

            if (!orderExists)
                return null;

            var orderItems = dtos.Select(dto => new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = dto.ProductId,
                ItemType = dto.ItemType,
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            }).ToList();

            await _context.OrderItems.AddRangeAsync(orderItems);
            await _context.SaveChangesAsync();

            return orderItems.Select(ToDto).ToList();
        }

        public async Task<OrderItemDto?> UpdateOrderItem(Guid orderId, Guid storeId, Guid id, OrderItemDto dto)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(r => r.Id == id && r.OrderId == orderId && r.Order.StoreId == storeId);

            if (orderItem is null)
                return null;

            orderItem.ProductId = dto.ProductId;
            orderItem.ItemType = dto.ItemType;
            orderItem.Quantity = dto.Quantity;
            orderItem.UnitPrice = dto.UnitPrice;

            await _context.SaveChangesAsync();

            return ToDto(orderItem);
        }

        public async Task<bool> DeleteOrderItem(Guid orderId, Guid storeId, Guid id)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(r => r.Id == id && r.OrderId == orderId && r.Order.StoreId == storeId);

            if (orderItem is null)
                return false;

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAllOrderItems(Guid orderId, Guid storeId)
        {
            bool orderExists = await _context.Orders
                .AnyAsync(o => o.Id == orderId && o.StoreId == storeId);

            if (!orderExists)
                return false;

            var rows = _context.OrderItems
                .Where(r => r.OrderId == orderId);

            _context.OrderItems.RemoveRange(rows);
            await _context.SaveChangesAsync();

            return true;
        }

        private static OrderItemDto ToDto(OrderItem r)
        {
            return new OrderItemDto
            {
                Id = r.Id,
                OrderId = r.OrderId,
                ProductId = r.ProductId,
                ItemType = r.ItemType,
                Quantity = r.Quantity,
                UnitPrice = r.UnitPrice
            };
        }
    }
}
