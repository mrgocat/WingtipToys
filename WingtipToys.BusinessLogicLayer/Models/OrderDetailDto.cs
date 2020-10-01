using System.ComponentModel.DataAnnotations;

namespace WingtipToys.BusinessLogicLayer.Models
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Username { get; set; }
        [Required]
        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }

        [Required]
        public int ProductId { get; set; }

        public string ProductName { get; set; }
        public string ImagePath { get; set; }

    }
}
