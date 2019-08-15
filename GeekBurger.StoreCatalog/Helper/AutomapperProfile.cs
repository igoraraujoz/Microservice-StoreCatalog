using AutoMapper;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.Ingredients.Contract.Response;
using System;

namespace GeekBurger.StoreCatalog.Helper
{
    public class AutomapperProfile : Profile
    {
        /// <summary>
        /// Mapper contracts to models.
        /// </summary>
        public AutomapperProfile()
        {
            CreateMap<StoreCatalogToGet, Store>();
            CreateMap<ItemToGet, Item>();
            CreateMap<ProductToGet, Product>().AfterMap<MatchItemsFromRepository>();
            CreateMap<IngredientsToUpsert, Item>();
            CreateMap<Production.Contract.Production, Model.Production>()
                .ForMember(dest => dest.ProductionId, opt => opt.MapFrom(src => src.ProductionId))
                .ForMember(dest => dest.On, opt => opt.MapFrom(src => src.On))
                .AfterMap<MatchRestrictionsFromRepository>();
                

            CreateMap<string, Restrictions>().ConstructUsing(str => new Restrictions { Restriction = str, Id = Guid.NewGuid() });
            CreateMap<GeekBurger.Products.Contract.ProductChangedMessage, Model.ProductChangedEvent>();
        }
    }
}
