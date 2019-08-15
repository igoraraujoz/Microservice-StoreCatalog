using System;

namespace GeekBurger.StoreCatalog.Contract
{
    public class StoreCatalogReady
    {
        public Guid StoreId { get; set; }
        public bool Ready { get; set; }
    }
}
