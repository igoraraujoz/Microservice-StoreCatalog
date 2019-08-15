using System;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Repository.Interfaces
{
    public interface IProductionRepository
    {
        Model.Production GetProductionById(Guid productionId);
        bool Update(Model.Production production);
        bool Create(Model.Production production);
        IEnumerable<Model.Production> GetAll();
        IEnumerable<Model.Restrictions> GetRestrictionsByProductionId(Guid productionId);
    }
}
