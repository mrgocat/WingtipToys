using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipToys.DataAccessLayer
{
    public class WingtipContext : DbContext
    {
    //    private const string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\WORK\ORDERDYNAMICS\CODINGEXERCISE\DATA\WINGTIPTOYS.MDF;Integrated Security=True";

        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        public WingtipContext(DbContextOptions<WingtipContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
         //   optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasOne(p => p.Category);
            modelBuilder.Entity<Order>().HasMany(o => o.OrderDetails).WithOne().HasForeignKey(o => o.OrderId);
            modelBuilder.Entity<OrderDetail>().HasOne(o => o.Product);
            modelBuilder.Entity<CartItem>().HasOne(c => c.Product);
        }

        public object Entry<T>()
        {
            throw new NotImplementedException();
        }
    }
}
