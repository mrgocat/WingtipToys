using System;
using System.Collections.Generic;
using System.Text;

namespace WingtipToys.BusinessLogicLayer.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public double? UnitPrice { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
