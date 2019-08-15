using GeekBurger.StoreCatalog.Model;

namespace GeekBurger.StoreCatalog.Repository.Interfaces
{
    public interface IProductChangedEventRepository
    {
        bool Add(ProductChangedEvent productChangedEvent);
        bool Update(ProductChangedEvent productChangedEvent);
        bool Delete(ProductChangedEvent productChangedEvent);
        bool Maintain(ProductChangedEvent productChangedEvent);        
    }
}
