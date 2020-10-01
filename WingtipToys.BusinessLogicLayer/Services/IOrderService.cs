using System;
using System.Collections.Generic;
using System.Text;
using WingtipToys.BusinessLogicLayer.Models;

namespace WingtipToys.BusinessLogicLayer.Services
{
    public interface IOrderService
    {
        public List<OrderDto> GetOrderList(string userName);
        public OrderDto Get(int orderId);
        public int Add(OrderDto order);
        public void Delete(int orderId);
    }
}
