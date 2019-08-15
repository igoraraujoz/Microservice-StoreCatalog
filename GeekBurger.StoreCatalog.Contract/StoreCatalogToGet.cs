using System;
using System.Collections.Generic;
using System.Text;

namespace GeekBurger.StoreCatalog.Contract
{
    public class StoreCatalogToGet
    {
        public string StoreName { get; set; }
        public Guid UserId { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
