using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WingtipToys.BusinessLogicLayer.Models;
using WingtipToys.BusinessLogicLayer.Services;
using WingtipToys.DataAccessLayer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace WingtipToys.BusinessLogicLayer.UnitTest
{
    [TestClass]
    public class ICartServiceTests
    {
        public static TestContext _testContect;
    //    private WingtipContext _context;
        private IMapper _mapper;
        /*public List<CartItemDto> Get(string cartId);
        public string Add(CartItemDto dto);
        public void Patch(string cartId, string itemId, int quantity);
        public void Delete(string cartId, string itemId);
        public void Delete(string cartId);*/
        [ClassInitialize]
        public static void initClass(TestContext context)
        {
            _testContect = context;
        }

        [TestInitialize]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WingtipProfile());
            });
            _mapper = config.CreateMapper();
        }
        #region CartTest
        [TestMethod]
        public void Get_CartItemDto_List_by_CartId()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            Product product2 = new Product
            {
                Id = 2,
                ProductName = "Luxery Car",
                CategoryId = 1,
                Description = "Very expencive CAr",
                ImagePath = "abcd.png",
                UnitPrice = 150.1
            };

            var cartId = Guid.NewGuid().ToString();
            var data = new List<CartItem>
            {
                new CartItem { Id = Guid.NewGuid().ToString(), CartId = cartId
                    , ProductId = product1.Id, Product = product1, Created = DateTime.Now, Quantity = 1},
                new CartItem { Id = Guid.NewGuid().ToString(), CartId = cartId
                    , ProductId = product2.Id, Product = product2, Created = DateTime.Now, Quantity = 2}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<CartItem>>();
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<WingtipContext>(new DbContextOptionsBuilder<WingtipContext>().Options);
            mockContext.Setup(m => m.CartItems).Returns(mockSet.Object);

            // Act
            ICartService service = new CartService(mockContext.Object, _mapper);
            List<CartItemDto> list = service.Get(cartId);

            // Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(product1.ProductName, list[0].ProductName);
            Assert.AreEqual(product2.ProductName, list[1].ProductName);
        }
        [TestMethod]
        public void Add_CartItemDto_AddOne()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            var cartId = Guid.NewGuid().ToString();

            var dataProduct = new List<Product>
            {
                product1
            }.AsQueryable();

            var mockSetProducts = new Mock<DbSet<Product>>();
            mockSetProducts.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(dataProduct.Provider);
            mockSetProducts.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(dataProduct.Expression);
            mockSetProducts.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(dataProduct.ElementType);
            mockSetProducts.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(dataProduct.GetEnumerator());

            var data = new List<CartItem>
            {
                
            }.AsQueryable();
            var mockSet = new Mock<DbSet<CartItem>>();
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<WingtipContext>(new DbContextOptionsBuilder<WingtipContext>().Options);
            mockContext.Setup(m => m.Products).Returns(mockSetProducts.Object);
            mockContext.Setup(m => m.CartItems).Returns(mockSet.Object);

            ICartService service = new CartService(mockContext.Object, _mapper);

            CartItemDto cartDto = new CartItemDto
            {
                CartId = cartId,
                ProductId = product1.Id,
                Quantity = 3
            };

            // Act
            string result = service.Add(cartDto);

            // Assert
            Assert.AreEqual(cartId, result);
            mockSet.Verify(m => m.Add(It.IsAny<CartItem>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
        [TestMethod]
        public void Add_AddOne_UpdateIfExists()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Product = product1,
                Created = DateTime.Now,
                Quantity = 1
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase("WingtipToysDB").Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            CartItemDto cartDto = new CartItemDto
            {
                CartId = cartId,
                ProductId = product1.Id,
                Quantity = 3
            };

            // Act
            service.Add(cartDto);

            // Assert
            var tempList = context.CartItems.ToList();
            Assert.AreEqual(1, tempList.Count);
            Assert.AreEqual(cartItem.Id, tempList[0].Id);
            Assert.AreEqual(cartItem.CartId, tempList[0].CartId);
            Assert.AreEqual(cartItem.ProductId, tempList[0].ProductId);
            Assert.AreEqual(3, tempList[0].Quantity);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();
        }
        [TestMethod]
        public void Patch_Quantity_UpdateOne()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Product = product1,
                Created = DateTime.Now,
                Quantity = 1
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase("WingtipToysDB").Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            // Act
            service.Patch(cartItem.CartId, cartItem.Id, 3);

            // Assert
            CartItem item = context.CartItems.FirstOrDefault(c => c.Id == cartItem.Id);
            Assert.IsNotNull(item);
            Assert.AreEqual(3, item.Quantity);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();
        }
        [TestMethod]
        public void Delete_CartId_DeleteAll()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            Product product2 = new Product
            {
                Id = 2,
                ProductName = "Luxery Car",
                CategoryId = 1,
                Description = "Very expencive CAr",
                ImagePath = "abcd.png",
                UnitPrice = 150.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Created = DateTime.Now,
                Quantity = 1
            };
            CartItem cartItem2 = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product2.Id,
                Created = DateTime.Now,
                Quantity = 5
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase("WingtipToysDB").Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.Products.Add(product2);
            context.CartItems.Add(cartItem2);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            // Act
            service.Delete(cartItem.CartId);

            // Assert
            var tempList = context.CartItems.ToList();
            Assert.AreEqual(0, tempList.Count);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();
        }
        [TestMethod]
        public void Delete_CartId_ItemId_DeleteOne()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            Product product2 = new Product
            {
                Id = 2,
                ProductName = "Luxery Car",
                CategoryId = 1,
                Description = "Very expencive CAr",
                ImagePath = "abcd.png",
                UnitPrice = 150.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Created = DateTime.Now,
                Quantity = 1
            };
            CartItem cartItem2 = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product2.Id,
                Created = DateTime.Now,
                Quantity = 5
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase("WingtipToysDB").Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.Products.Add(product2);
            context.CartItems.Add(cartItem2);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            // Act
            service.Delete(cartItem.CartId, cartItem.Id);

            // Assert
            var tempList = context.CartItems.ToList();
            Assert.AreEqual(1, tempList.Count);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();
        }
        #endregion
        #region CartTestAsync
        [TestMethod]
        [TestCategory("Async")]
        public async Task GetAsync_CartItemDto_List_by_CartId()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            Product product2 = new Product
            {
                Id = 2,
                ProductName = "Luxery Car",
                CategoryId = 1,
                Description = "Very expencive CAr",
                ImagePath = "abcd.png",
                UnitPrice = 150.1
            };

            var cartId = Guid.NewGuid().ToString();
            var data = new List<CartItem>
            {
                new CartItem { Id = Guid.NewGuid().ToString(), CartId = cartId
                    , ProductId = product1.Id, Product = product1, Created = DateTime.Now, Quantity = 1},
                new CartItem { Id = Guid.NewGuid().ToString(), CartId = cartId
                    , ProductId = product2.Id, Product = product2, Created = DateTime.Now, Quantity = 2}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<CartItem>>();
            mockSet.As<IDbAsyncEnumerable<CartItem>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<CartItem>(data.GetEnumerator()));
            mockSet.As<IQueryable<CartItem>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<CartItem>(data.Provider));

            mockSet.As<IQueryable<CartItem>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<CartItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
        //    mockSet.As<IQueryable<CartItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<WingtipContext>(new DbContextOptionsBuilder<WingtipContext>().Options);
            mockContext.Setup(m => m.CartItems).Returns(mockSet.Object);

            // Act
            ICartService service = new CartService(mockContext.Object, _mapper);
            List<CartItemDto> list = await service.GetAsync(cartId);

            // Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(product1.ProductName, list[0].ProductName);
            Assert.AreEqual(product2.ProductName, list[1].ProductName);
        }
        [TestMethod]
        [TestCategory("Async")]
        public async Task AddAsync_CartItemDto_AddOne()
        {
            string dbName = "AddAsync_CartItemDto_AddOne";
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            var cartId = Guid.NewGuid().ToString();

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase("WingtipToysDB").Options);

            context.Products.Add(product1);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            CartItemDto cartDto = new CartItemDto
            {
                CartId = cartId,
                ProductId = product1.Id,
                Quantity = 3
            };

            // Act
            string result = await service.AddAsync(cartDto);

            // Assert
            Assert.AreEqual(cartId, result);
        }
        [TestMethod]
        [TestCategory("Async")]
        public async Task AddAsync_AddOne_UpdateIfExists()
        {
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Product = product1,
                Created = DateTime.Now,
                Quantity = 1
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase("WingtipToysDB").Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            CartItemDto cartDto = new CartItemDto
            {
                CartId = cartId,
                ProductId = product1.Id,
                Quantity = 3
            };

            // Act
            await service.AddAsync(cartDto);

            // Assert
            var tempList = context.CartItems.ToList();
            Assert.AreEqual(1, tempList.Count);
            Assert.AreEqual(cartItem.Id, tempList[0].Id);
            Assert.AreEqual(cartItem.CartId, tempList[0].CartId);
            Assert.AreEqual(cartItem.ProductId, tempList[0].ProductId);
            Assert.AreEqual(3, tempList[0].Quantity);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();
        }
        [TestMethod]
        [TestCategory("Async")]
        public async Task PatchAsync_Quantity_UpdateOne()
        {
            string dbName = "PatchAsync_Quantity_UpdateOne";
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Product = product1,
                Created = DateTime.Now,
                Quantity = 1
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase(dbName).Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            // Act
            await service.PatchAsync(cartItem.CartId, cartItem.Id, 3);

            // Assert
            CartItem item = context.CartItems.FirstOrDefault(c => c.Id == cartItem.Id);
            Assert.IsNotNull(item);
            Assert.AreEqual(3, item.Quantity);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();
        }
        [TestMethod]
        [TestCategory("Async")]
        public async Task DeleteAsync_CartId_DeleteAll()
        {
            string dbName = "DeleteAsync_CartId_DeleteAll";
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            Product product2 = new Product
            {
                Id = 2,
                ProductName = "Luxery Car",
                CategoryId = 1,
                Description = "Very expencive CAr",
                ImagePath = "abcd.png",
                UnitPrice = 150.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Created = DateTime.Now,
                Quantity = 1
            };
            CartItem cartItem2 = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product2.Id,
                Created = DateTime.Now,
                Quantity = 5
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase(dbName).Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.Products.Add(product2);
            context.CartItems.Add(cartItem2);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            // Act
            await service.DeleteAsync(cartItem.CartId);

            // Assert
            var tempList = context.CartItems.ToList();
            Assert.AreEqual(0, tempList.Count);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();
        }
        [TestMethod]
        [TestCategory("Async")]
        public async Task DeleteAsync_CartId_ItemId_DeleteOne()
        {
            string dbName = "DeleteAsync_CartId_ItemId_DeleteOne";
            // Arrange
            Product product1 = new Product
            {
                Id = 1,
                ProductName = "Racing Car",
                CategoryId = 1,
                Description = "Nice Racing Car",
                ImagePath = "abc.png",
                UnitPrice = 23.1
            };
            Product product2 = new Product
            {
                Id = 2,
                ProductName = "Luxery Car",
                CategoryId = 1,
                Description = "Very expencive CAr",
                ImagePath = "abcd.png",
                UnitPrice = 150.1
            };
            var cartId = Guid.NewGuid().ToString();
            CartItem cartItem = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product1.Id,
                Created = DateTime.Now,
                Quantity = 1
            };
            CartItem cartItem2 = new CartItem
            {
                Id = Guid.NewGuid().ToString(),
                CartId = cartId,
                ProductId = product2.Id,
                Created = DateTime.Now,
                Quantity = 5
            };

            var context = new WingtipContext(new DbContextOptionsBuilder<WingtipContext>().UseInMemoryDatabase(dbName).Options);

            context.Products.Add(product1);
            context.CartItems.Add(cartItem);
            context.Products.Add(product2);
            context.CartItems.Add(cartItem2);
            context.SaveChanges();

            ICartService service = new CartService(context, _mapper);

            // Act
            await service.DeleteAsync(cartItem.CartId, cartItem.Id);

            // Assert
            var tempList = context.CartItems.ToList();
            Assert.AreEqual(1, tempList.Count);

            // Clean data
            context.Products.RemoveRange(context.Products);
            context.CartItems.RemoveRange(context.CartItems);
            context.SaveChanges();
            context.Dispose();

        }
        #endregion
    }
    #region InternalClassesForAsync
    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider, IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return ExecuteAsync<TResult>(expression, cancellationToken).Result;
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>, IAsyncEnumerable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>, IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public ValueTask<bool> MoveNextAsync()
        {
            var result = MoveNextAsync(CancellationToken.None);
            return new ValueTask<bool>(result);
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
    #endregion
}
