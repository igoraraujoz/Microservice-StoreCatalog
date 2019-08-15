using AutoMapper;

namespace GeekBurger.StoreCatalog.Helper
{
    public class MatchRestrictionsFromRepository : IMappingAction<Production.Contract.Production, Model.Production>
    {        
        public void Process(Production.Contract.Production source, Model.Production destination)
        {
            foreach (var item in destination.Restrictions)
            {
                item.ProductionId = source.ProductionId;
            }
        }
    }
}
