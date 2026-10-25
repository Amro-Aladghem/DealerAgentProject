using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto> CreateCart(Guid storeId)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                StoreId = storeId
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return ToDto(cart);
        }

        public async Task<CartDto?> GetCartById(Guid storeId, Guid id)
        {
            return await _context.Carts
                .Where(c => c.Id == id && c.StoreId == storeId)
                .Select(c => ToDto(c))
                .FirstOrDefaultAsync();
        }

        public async Task<CartDto?> GetCartForStore(Guid storeId)
        {
            return await _context.Carts
                .Where(c => c.StoreId == storeId)
                .Select(c => ToDto(c))
                .FirstOrDefaultAsync();
        }

        public async Task<List<CartDto>> GetCartsForStore(Guid storeId)
        {
            return await _context.Carts
                .Where(c => c.StoreId == storeId)
                .Select(c => ToDto(c))
                .ToListAsync();
        }

        public async Task<bool> DeleteCart(Guid storeId, Guid id)
        {
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == id && c.StoreId == storeId);

            if (cart is null)
                return false;

            var rows = _context.CartItems
                .Where(r => r.CartId == id);

            _context.CartItems.RemoveRange(rows);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return true;
        }

        private static CartDto ToDto(Cart c)
        {
            return new CartDto
            {
                Id = c.Id,
                StoreId = c.StoreId
            };
        }
    }
}
