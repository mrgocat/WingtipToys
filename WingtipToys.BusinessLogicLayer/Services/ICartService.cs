using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WingtipToys.BusinessLogicLayer.Models;

namespace WingtipToys.BusinessLogicLayer.Services
{
    public interface ICartService
    {
        public List<CartItemDto> Get(string cartId);
        public string Add(CartItemDto dto);
        public void Patch(string cartId, string itemId, int quantity);
        public void Delete(string cartId, string itemId);
        public void Delete(string cartId);

        public Task<List<CartItemDto>> GetAsync(string cartId);
        public Task<string> AddAsync(CartItemDto dto);
        public Task PatchAsync(string cartId, string itemId, int quantity);
        public Task DeleteAsync(string cartId, string itemId);
        public Task DeleteAsync(string cartId);
    }
}
