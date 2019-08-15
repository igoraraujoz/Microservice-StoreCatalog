using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Model;

namespace GeekBurger.StoreCatalog.Helper
{
    public class MatchItemsFromRepository : IMappingAction<ProductToGet, Product>
    {
        public void Process(ProductToGet source, Product destination)
        {
            foreach (var item in destination.Items)
            {
                item.ProductId = destination.ProductId;
            }
        }
    }
}
