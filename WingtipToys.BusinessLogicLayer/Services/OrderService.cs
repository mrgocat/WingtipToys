using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.DataAccessLayer;

namespace WingtipToys.BusinessLogicLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly WingtipContext _context;
        private readonly IMapper _mapper;

        public OrderService(WingtipContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int Add(OrderDto orderDto)
        {
            Order order = _mapper.Map<Order>(orderDto);

            // check products before update
            foreach(OrderDetail detail in order.OrderDetails)
            {
                Product product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == detail.ProductId);
                if(product == null)
                {
                    throw new KeyNotFoundException($"The product with ID={detail.ProductId} was not found.");
                }
            }
            order.HasBeenShipped = false;
            order.OrderDate = DateTime.Now;
        //    order.PaymentTransactionId = ""; // TO DO  
            
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        public OrderDto Get(int orderId)
        {
            Order order = _context.Orders.AsNoTracking().Include(o => o.OrderDetails).ThenInclude(o => o.Product).FirstOrDefault(o => o.Id == orderId);
            if(order == null)
            {
                return null;
            //    throw new KeyNotFoundException($"A record with ID={orderId} was not found.");
            }
            return _mapper.Map<OrderDto>(order);
        }
        public List<OrderDto> GetOrderList(string userName)
        {
            List<Order> list = _context.Orders.AsNoTracking().Where(e => e.Username == userName).ToList();
            return _mapper.Map<List<OrderDto>>(list);
        }
        public void Delete(int orderId)
        {
            Order order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                _context.Entry(order).State = EntityState.Deleted;
                _context.SaveChanges();
            }
        }


        public async Task<int> AddAsync(OrderDto orderDto)
        {
            Order order = _mapper.Map<Order>(orderDto);

            // check products before update
            foreach (OrderDetail detail in order.OrderDetails)
            {
                Product product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == detail.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"The product with ID={detail.ProductId} was not found.");
                }
            }
            order.HasBeenShipped = false;
            order.OrderDate = DateTime.Now;
            //    order.PaymentTransactionId = ""; // TO DO  

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<OrderDto> GetAsync(int orderId)
        {
            Order order = await _context.Orders.AsNoTracking().Include(o => o.OrderDetails).ThenInclude(o => o.Product).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                return null;
                //    throw new KeyNotFoundException($"A record with ID={orderId} was not found.");
            }
            return _mapper.Map<OrderDto>(order);
        }
        public async Task<List<OrderDto>> GetOrderListAsync(string userName)
        {
            List<Order> list = await _context.Orders.AsNoTracking().Where(e => e.Username == userName).ToListAsync();
            return _mapper.Map<List<OrderDto>>(list);
        }
        public async Task DeleteAsync(int orderId)
        {
            Order order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order != null)
            {
                _context.Entry(order).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }
    }
}
