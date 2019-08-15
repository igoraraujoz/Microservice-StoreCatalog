using GeekBurger.StoreCatalog.Model;
using System;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Repository
{
    public interface IProductsRepository
    {
        Product GetProductById(Guid productId);        
        bool Update(Product product);
        bool Create(Product product);
        IEnumerable<Product> GetAll();
        IEnumerable<Item> GetItemsByProductId(Guid productId);
    }
}
