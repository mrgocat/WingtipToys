using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WingtipToys.DataAccessLayer
{
    public class CartItem
    {
        [Column("ItemId")]
        public string Id { get; set; }
        public string CartId { get; set; }
        public int Quantity { get; set; }
        [Column("DateCreated")]
        public DateTime Created { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
