using Application.Dtos;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CartItemService
    {
        private readonly AppDbContext _context;

        public CartItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartItemDto>> GetCartItemsForCart(Guid cartId, Guid storeId)
        {
            return await _context.CartItems
                .Where(r => r.CartId == cartId && r.Cart.StoreId == storeId)
                .Select(r => ToDto(r))
                .ToListAsync();
        }

        public async Task<CartItemDto?> GetCartItemById(Guid cartId, Guid storeId, Guid id)
        {
            return await _context.CartItems
                .Where(r => r.Id == id && r.CartId == cartId && r.Cart.StoreId == storeId)
                .Select(r => ToDto(r))
                .FirstOrDefaultAsync();
        }

        public async Task<CartItemDto?> AddCartItem(Guid cartId, Guid storeId, CartItemDto dto)
        {
            bool cartExists = await _context.Carts
                .AnyAsync(c => c.Id == cartId && c.StoreId == storeId);

            if (!cartExists)
                return null;

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = dto.ProductId,
                ItemType = dto.ItemType,
                Quantity = dto.Quantity
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return ToDto(cartItem);
        }

        public async Task<List<CartItemDto>?> AddCartItems(Guid cartId, Guid storeId, List<CartItemDto> dtos)
        {
            bool cartExists = await _context.Carts
                .AnyAsync(c => c.Id == cartId && c.StoreId == storeId);

            if (!cartExists)
                return null;

            var cartItems = dtos.Select(dto => new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cartId,
                ProductId = dto.ProductId,
                ItemType = dto.ItemType,
                Quantity = dto.Quantity
            }).ToList();

            await _context.CartItems.AddRangeAsync(cartItems);
            await _context.SaveChangesAsync();

            return cartItems.Select(ToDto).ToList();
        }

        public async Task<CartItemDto?> UpdateCartItem(Guid cartId, Guid storeId, Guid id, CartItemDto dto)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(r => r.Id == id && r.CartId == cartId && r.Cart.StoreId == storeId);

            if (cartItem is null)
                return null;

            cartItem.ProductId = dto.ProductId;
            cartItem.ItemType = dto.ItemType;
            cartItem.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();

            return ToDto(cartItem);
        }

        public async Task<bool> DeleteCartItem(Guid cartId, Guid storeId, Guid id)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(r => r.Id == id && r.CartId == cartId && r.Cart.StoreId == storeId);

            if (cartItem is null)
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAllCartItems(Guid cartId, Guid storeId)
        {
            bool cartExists = await _context.Carts
                .AnyAsync(c => c.Id == cartId && c.StoreId == storeId);

            if (!cartExists)
                return false;

            var rows = _context.CartItems
                .Where(r => r.CartId == cartId);

            _context.CartItems.RemoveRange(rows);
            await _context.SaveChangesAsync();

            return true;
        }

        private static CartItemDto ToDto(CartItem r)
        {
            return new CartItemDto
            {
                Id = r.Id,
                CartId = r.CartId,
                ProductId = r.ProductId,
                ItemType = r.ItemType,
                Quantity = r.Quantity
            };
        }
    }
}
