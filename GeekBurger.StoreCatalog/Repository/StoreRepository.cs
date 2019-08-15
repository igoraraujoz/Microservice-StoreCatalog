using System;
using System.Linq;
using GeekBurger.StoreCatalog.Model;

namespace GeekBurger.StoreCatalog.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private StoreCatalogDbContext _context { get; set; }

        public StoreRepository(StoreCatalogDbContext context)
        {
            _context = context;
        }

        public Store GetStoreByName(string name)
        {
            return _context.Stores.FirstOrDefault(store => store.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool Create(Store store)
        {
            _context.Stores.Add(store);
            return (_context.SaveChanges() > 0);
        }

        public Store GetStore()
        {
            return _context.Stores.FirstOrDefault();
        }
    }
}
