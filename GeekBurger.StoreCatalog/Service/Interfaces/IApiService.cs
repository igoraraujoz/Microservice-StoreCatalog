using GeekBurger.Products.Contract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Service.Interfaces
{
    public interface IApiService
    {
        Task<List<ProductToGet>> GetProducts(string storeName);
        Task<List<Production.Contract.Production>> GetProductionAreas();
        Task<List<GeekBurger.Ingredients.Contract.Response.IngredientsToUpsert>> GetProductsByRestrictions(Guid storeId, string restrictions);
    }
}
