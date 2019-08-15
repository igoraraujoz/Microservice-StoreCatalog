using System;
using System.Collections.Generic;
using System.Linq;
using GeekBurger.StoreCatalog.Model;

namespace GeekBurger.StoreCatalog.Repository
{
    public class ProductRepository : IProductsRepository
    {
        private StoreCatalogDbContext _context { get; set; }

        public ProductRepository(StoreCatalogDbContext context)
        {
            _context = context;
        }

        public Product GetProductById(Guid productId)
        {
            return _context.Products.FirstOrDefault(product => product.ProductId == productId);
        }

        public bool Load()
        {
            throw new NotImplementedException();
        }

        public bool Update(Product product)
        {
            _context.Products.Update(product);
            return (_context.SaveChanges() > 0);
        }

        public bool Create(Product product)
        {
            _context.Products.Add(product);
            return (_context.SaveChanges() > 0);
        }

        public IEnumerable<Product> GetAll()
        {
            var products = _context.Products.ToList();

            foreach (var item in products)
            {
                item.Items = GetItemsByProductId(item.ProductId).ToList();
            }

            return products;
        }

        public IEnumerable<Item> GetItemsByProductId(Guid productId)
        {            
            return _context.Items.Where(x => x.ProductId == productId).ToList(); 
        }
    }
}
