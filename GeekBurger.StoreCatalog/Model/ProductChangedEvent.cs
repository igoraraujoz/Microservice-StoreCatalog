using GeekBurger.Products.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Model
{
    public class ProductChangedEvent
    {    
        public Guid EventId { get; set; }
        public ProductState State { get; set; }        
        public Product Product { get; set; }
        public bool MessageSent { get; set; }
    }
}
