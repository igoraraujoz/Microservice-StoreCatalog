using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.StoreCatalog.Model
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }
        public string Name { get; set; }

        [ForeignKey("ProductId")]
        public Guid ProductId { get; set; }
    }
}
