using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WingtipToys.DataAccessLayer
{
    public class Order
    {
        [Column("OrderId")]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Total { get; set; }
        public string PaymentTransactionId { get; set; }
        public bool HasBeenShipped { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
