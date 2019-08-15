using System;
using System.ComponentModel.DataAnnotations;

namespace GeekBurger.StoreCatalog.Model
{
    public class Store
    {
        [Key]
        public Guid StoreId { get; set; }
        public string Name { get; set; }
    }
}
