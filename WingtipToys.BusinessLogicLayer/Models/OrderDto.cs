using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WingtipToys.BusinessLogicLayer.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public decimal Total { get; set; }
        public string PaymentTransactionId { get; set; }
        public bool HasBeenShipped { get; set; }

        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
