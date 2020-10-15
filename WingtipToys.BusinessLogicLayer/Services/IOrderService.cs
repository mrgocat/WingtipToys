using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WingtipToys.BusinessLogicLayer.Models;

namespace WingtipToys.BusinessLogicLayer.Services
{
    public interface IOrderService
    {
        public List<OrderDto> GetOrderList(string userName);
        public OrderDto Get(int orderId);
        public int Add(OrderDto order);
        public void Delete(int orderId);

        public Task<List<OrderDto>> GetOrderListAsync(string userName);
        public Task<OrderDto> GetAsync(int orderId);
        public Task<int> AddAsync(OrderDto order);
        public Task DeleteAsync(int orderId);

    }
}
