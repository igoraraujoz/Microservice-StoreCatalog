using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using System;
using System.Linq;

namespace GeekBurger.StoreCatalog.Repository
{
    public class ProductChangedEventRepository : IProductChangedEventRepository
    {
        private StoreCatalogDbContext _context { get; set; }

        public ProductChangedEventRepository(StoreCatalogDbContext context)
        {
            _context = context;
        }
        public bool Add(ProductChangedEvent productChangedEvent)
        {
            if(productChangedEvent.Product.ProductId != null)
                
            _context.Add(productChangedEvent.Product);
            return (_context.SaveChanges() > 0);
        }

        public bool Delete(ProductChangedEvent productChangedEvent)
        {
            _context.Products.Remove(productChangedEvent.Product);
            return (_context.SaveChanges() > 0);
        }        

        public bool Update(ProductChangedEvent productChangedEvent)
        {
             _context.Products.Update(productChangedEvent.Product);
            return (_context.SaveChanges() > 0);
        }

        /// <summary>
        /// Method responsable to Add/Update/Delete follows rules and persisting data in memory.
        /// </summary>
        /// <param name="productChangedEvent"></param>
        /// <returns></returns>
        public bool Maintain(ProductChangedEvent productChangedEvent)
        {            
            if (productChangedEvent.State == ProductState.Added || productChangedEvent.State == ProductState.Modified)
            {
                if (productChangedEvent.Product.ProductId != Guid.Empty)
                {
                    var product = _context.Products.Where(x => x.ProductId.Equals(productChangedEvent.Product.ProductId));

                    if (product.Equals(null))                    
                        this.Update(productChangedEvent); //Se existir o Id na base, atualiza o produto, senão cria.                    
                    else                    
                        this.Add(productChangedEvent);

                    return true;
                }
                else
                    return false;
            }
            else if (productChangedEvent.State == ProductState.Deleted)
            {
                if (productChangedEvent.Product.ProductId != Guid.Empty)
                {
                    var product = _context.Products.Where(x => x.ProductId == productChangedEvent.Product.ProductId);

                    if (product != null)
                        _context.Remove(productChangedEvent.Product);

                    return true;
                }
                else
                    return false;
            }            

            return false;
        }
    }
}
