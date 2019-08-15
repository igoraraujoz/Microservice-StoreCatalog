using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekBurger.StoreCatalog.Extension
{
    public static class StoreCatalogContextExtensions
    {
        public static void Seed(this StoreCatalogDbContext dbContext)
        {
            if (dbContext.Stores.Any())
                return;

            dbContext.Stores.AddRange(new List<Store> {
                new Store { Name = "California - Pasadena", StoreId = new Guid("8048e9ec-80fe-4bad-bc2a-e4f4a75c834e") },
                new Store { Name = "Los Angeles - Beverly Hills", StoreId = new Guid("8d618778-85d7-411e-878b-846a8eef30c0") }
            });

            dbContext.SaveChanges();
        }
    }
}
