using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WingtipToys.DataAccessLayer
{
    public class Category
    {
        [Column("CategoryID")]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

    }
}