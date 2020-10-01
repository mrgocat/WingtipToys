using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.DataAccessLayer;

namespace WingtipToys.BusinessLogicLayer.Services
{
    public class CartService : ICartService
    {
        private readonly WingtipContext _context;
        private readonly IMapper _mapper;

        public CartService(WingtipContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<CartItemDto> Get(string cartId)
        {
            var list = _context.CartItems.AsNoTracking().Include(c => c.Product).Where(e => e.CartId == cartId).ToList();
            var dtoList = _mapper.Map<List<CartItemDto>>(list);
            return dtoList;
        }
        public string Add(CartItemDto dto)
        {
            if (dto.Quantity <= 0) dto.Quantity = 1;
            var product = _context.Products.FirstOrDefault(p => p.Id == dto.ProductId);
            if(product == null)
            {
                throw new KeyNotFoundException($"The product with ID={dto.ProductId} was not found.");
            }
            CartItem cartItem = null;
            Guid x;

            if(!Guid.TryParse(dto.CartId, out x))
            {
                dto.CartId = Guid.NewGuid().ToString();
            }
            else { 
                cartItem = _context.CartItems.FirstOrDefault(c => c.CartId == dto.CartId && c.ProductId == dto.ProductId);
            }

            if (cartItem == null)
            { // create new
                cartItem = new CartItem
                {
                    Id = Guid.NewGuid().ToString(),
                    CartId = dto.CartId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    Created = DateTime.Now
                };
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
            }
            else
            {
                cartItem.Quantity = dto.Quantity;
                _context.SaveChanges();
            }
            return cartItem.CartId;
        }

        public void Patch(string cartId, string itemId, int quantity)
        {
            var cartItem = _context.CartItems.FirstOrDefault(c => c.CartId == cartId && c.Id == itemId);
            if(cartItem != null)
            {
                cartItem.Quantity = quantity;
                _context.Entry(cartItem).Property("Quantity").IsModified = true;
                _context.SaveChanges();
            }
        }
        public void Delete(string cartId, string itemId)
        {
            var cartItem = _context.CartItems.FirstOrDefault(c => c.CartId == cartId && c.Id == itemId);
            if (cartItem != null)
            {
                _context.Entry(cartItem).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }
        public void Delete(string cartId)
        {
            var cartItems = _context.CartItems.Where(c => c.CartId == cartId).ToList();
            foreach (var cartItem in cartItems)
            {
                _context.Entry(cartItem).State = EntityState.Deleted;
            }
            _context.SaveChanges();
        }
    }
}
