using GeekBurger.StoreCatalog.Model;

namespace GeekBurger.StoreCatalog.Repository
{
    public interface IStoreRepository
    {
        Store GetStoreByName(string name);
        bool Create(Store store);

        Store GetStore();
    }
}
