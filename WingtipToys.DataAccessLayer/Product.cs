using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WingtipToys.DataAccessLayer
{
    public class Product
    {
        [Column("ProductID")]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double? UnitPrice { get; set; }
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public object Include()
        {
            throw new NotImplementedException();
        }
    }
}