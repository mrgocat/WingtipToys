using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WingtipToys.BusinessLogicLayer;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.BusinessLogicLayer.Services;
using WingtipToys.DataAccessLayer;

namespace WingtipToys.UnitTest
{
    [TestClass]
    public class BusinessLogicLayerTest
    {
        private WingtipContext _context;
        private IMapper _mapper;
        [TestInitialize]
        public void Setup()
        {
            //    DbContextOptions<WingtipContext> options;
            var builder = new DbContextOptionsBuilder<WingtipContext>();
            builder.UseSqlServer(Constants.ConnectionString);
            _context = new WingtipContext(builder.Options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WingtipProfile());
            });
            _mapper = config.CreateMapper();
        }
        [TestMethod]
        public void ProductServiceTest()
        {
            IProductService productService = new ProductService(_context, _mapper);
            var products = productService.GetProductList();
            Assert.IsNotNull(products);
            foreach (var item in products)
            {
                Assert.IsNotNull(item.ProductName);
                var product = productService.Get(item.Id);
                Assert.IsNotNull(product);
                Assert.IsNotNull(product.CategoryName);
            }
        }
        [TestMethod]
        public void CartServiceTest()
        {
            ICartService cartService = new CartService(_context, _mapper);
            string cartId = "f24c3e54-4d90-4e2e-ac08-f663c4ecc7a2";

            List<CartItemDto> cartList = cartService.Get(cartId);
            if (cartList.Count > 0)
            {
                foreach (CartItemDto dto in cartList)
                {
                    cartService.Delete(cartId, dto.Id);
                }
            }

            CartItemDto cartDto1 = new CartItemDto
            {
                CartId = cartId,
                ProductId = 10,
                Quantity = 3
            };
            cartService.Add(cartDto1);
            CartItemDto cartDto2 = new CartItemDto
            {
                CartId = cartId,
                ProductId = 11,
                Quantity = 6
            };
            cartService.Add(cartDto2);
            int chk = 0;
            cartList = cartService.Get(cartId);
            foreach (CartItemDto item in cartList)
            {
                Assert.IsNotNull(item.Id);
                if (item.ProductId == cartDto1.ProductId)
                {
                    cartDto1.Id = item.Id;
                    Assert.AreEqual(cartDto1.Quantity, item.Quantity);
                    chk++;
                }
                if (item.ProductId == cartDto2.ProductId)
                {
                    cartDto2.Id = item.Id;
                    Assert.AreEqual(cartDto2.Quantity, item.Quantity);
                    chk++;
                }
            }
            Assert.IsTrue(chk == 2);
            cartDto1.Quantity = 1;
            cartService.Patch(cartId, cartDto1.Id, cartDto1.Quantity);

            cartDto2.Quantity = 4;
            cartService.Patch(cartId, cartDto2.Id, cartDto2.Quantity);

            chk = 0;
            cartList = cartService.Get(cartId);
            foreach (CartItemDto item in cartList)
            {
                if (item.ProductId == cartDto1.ProductId)
                {
                    Assert.AreEqual(cartDto1.Quantity, item.Quantity);
                    chk++;
                }
                if (item.ProductId == cartDto2.ProductId)
                {
                    Assert.AreEqual(cartDto2.Quantity, item.Quantity);
                    chk++;
                }
            }
            Assert.IsTrue(chk == 2);

            cartService.Delete(cartId, cartDto1.Id);
            cartService.Delete(cartId, cartDto2.Id);

            cartList = cartService.Get(cartId);
            Assert.IsTrue(cartList.Count == 0);

            CartItemDto cartDto3 = new CartItemDto
            {
                ProductId = 10,
                Quantity = 3
            };
            cartId = cartService.Add(cartDto3);
            Assert.IsNotNull(cartId);
            cartList = cartService.Get(cartId);
            Assert.IsTrue(cartList.Count == 1);
            cartService.Delete(cartId);
            cartList = cartService.Get(cartId);
            Assert.IsTrue(cartList.Count == 0);
        }
        [TestMethod]
        public void OrderServiceTest()
        {
            IOrderService orderService = new OrderService(_context, _mapper);
            OrderDto dto = new OrderDto
            {
                Username = "raykim.kerei@gmail.com",
                FirstName = "Ray",
                LastName = "Kim",
                Address = "33 BrownMoose pl",
                City = "Whitby",
                State = "ON",
                PostalCode = "L2R 2W3",
                Country = "Canada",
                Phone = "416-123-4567",
                Email = "raykim.kerei@gmail.com",
                Total = 193.85M,
                OrderDetails = new List<OrderDetailDto>
                {
                    new OrderDetailDto
                    {
                        Username = "raykim.kerei@gmail.com",
                        ProductId = 9,
                        UnitPrice = 32.95,
                        Quantity = 3
                    },
                    new OrderDetailDto
                    {
                        Username = "raykim.kerei@gmail.com",
                        ProductId = 13,
                        UnitPrice = 95,
                        Quantity = 1
                    }
                }
            };
            int orderId = orderService.Add(dto);
            Assert.IsTrue(orderId > 0);
            OrderDto tmpOrder = orderService.Get(orderId);
            Assert.IsNotNull(tmpOrder);
            Assert.AreEqual(dto.Username, tmpOrder.Username);
            Assert.AreEqual(dto.FirstName, tmpOrder.FirstName);
            Assert.AreEqual(dto.LastName, tmpOrder.LastName);
            Assert.AreEqual(dto.Address, tmpOrder.Address);
            Assert.AreEqual(dto.City, tmpOrder.City);
            Assert.AreEqual(dto.State, tmpOrder.State);
            Assert.AreEqual(dto.PostalCode, tmpOrder.PostalCode);
            Assert.AreEqual(dto.Phone, tmpOrder.Phone);
            Assert.AreEqual(dto.Email, tmpOrder.Email);
            Assert.AreEqual(dto.Total, tmpOrder.Total);
            Assert.IsNotNull(tmpOrder.OrderDetails);
            
            Assert.IsTrue(dto.OrderDetails.Count == tmpOrder.OrderDetails.Count);

            var orderList = orderService.GetOrderList(dto.Username);
            Assert.IsTrue(orderList.Count > 0);
            int chk = 0;
            foreach(OrderDto o in orderList)
            {
                Assert.AreEqual(dto.Username, o.Username);
                if(o.Id == orderId)
                {
                    chk++;
                }
            }
            Assert.IsTrue(chk == 1);

            orderService.Delete(orderId);
            tmpOrder = orderService.Get(orderId);
            Assert.IsNull(tmpOrder);
        }
    }
}
