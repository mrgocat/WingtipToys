using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WingtipToys.BusinessLogicLayer.Models
{
    public class CartItemDto
    {
        public string Id { get; set; }
        public string CartId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime Created { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImagePath { get; set; }
        public double? UnitPrice { get; set; }
    }
}
