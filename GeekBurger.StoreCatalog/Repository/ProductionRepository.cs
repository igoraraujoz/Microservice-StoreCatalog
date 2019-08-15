using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekBurger.StoreCatalog.Repository
{
    public class ProductionRepository : IProductionRepository
    {
        private StoreCatalogDbContext _context { get; set; }

        public ProductionRepository(StoreCatalogDbContext context)
        {
            _context = context;
        }

        public bool Create(Model.Production production)
        {
            _context.Productions.Add(production);
            return (_context.SaveChanges() > 0);
        }

        public Model.Production GetProductionById(Guid productionId)
        {
            return _context.Productions.FirstOrDefault(production => production.ProductionId == productionId);
        }

        public bool Update(Model.Production production)
        {
            _context.Productions.Update(production);
            return (_context.SaveChanges() > 0);
        }

        public IEnumerable<Model.Production> GetAll()
        {
            var productions = _context.Productions.ToList();

            foreach (var item in productions)
            {
                item.Restrictions = GetRestrictionsByProductionId(item.ProductionId).ToList();
            }
            return productions;
        }

        public IEnumerable<Restrictions> GetRestrictionsByProductionId(Guid productionId)
        {
            return _context.Restrictions.Where(x => x.ProductionId == productionId);
        }
    }
}
