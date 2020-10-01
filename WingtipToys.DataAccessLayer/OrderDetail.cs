using System.ComponentModel.DataAnnotations.Schema;

namespace WingtipToys.DataAccessLayer
{
    public class OrderDetail
    {
        [Column("OrderDetailId")]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Username { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }
    }
}