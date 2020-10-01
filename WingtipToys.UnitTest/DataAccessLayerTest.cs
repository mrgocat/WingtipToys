using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WingtipToys.DataAccessLayer;

namespace WingtipToys.UnitTest
{
    [TestClass]
    public class DataAccessLayerTest
    {
        private WingtipContext _context;
        [TestInitialize]
        public void Setup()
        {
            //    DbContextOptions<WingtipContext> options;
            var builder = new DbContextOptionsBuilder<WingtipContext>();
            builder.UseSqlServer(Constants.ConnectionString);
            _context = new WingtipContext(builder.Options);
        }
        [TestMethod]
        public void WingtipReadTest()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            Assert.IsNotNull(products);
            foreach (var product in products)
            {
                Assert.IsNotNull(product.Category.CategoryName);
            }
            var cartItems = _context.CartItems.Include(c => c.Product).ToList();
            Assert.IsNotNull(cartItems);
            foreach (var item in cartItems)
            {
                Assert.IsNotNull(item.Product.ProductName);
            }
            var categories = _context.Categories.ToList();
            Assert.IsNotNull(categories);
            var orders = _context.Orders.Include(o => o.OrderDetails).ToList();
            Assert.IsNotNull(orders);
            foreach (var order in orders)
            {
                Assert.IsNotNull(order.OrderDetails);
            }
            var orderDetails = _context.OrderDetails.Include(o => o.Product).ToList();
            Assert.IsNotNull(orderDetails);
            foreach (var detail in orderDetails)
            {
                Assert.IsNotNull(detail.Product.ProductName);
            }
        }
    }
}
