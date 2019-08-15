using System;
using System.Collections.Generic;
using System.Linq;
using GeekBurger.StoreCatalog.Model;
using Microsoft.AspNetCore.Mvc;
using GeekBurger.StoreCatalog.Service.Interfaces;
using AutoMapper;
using GeekBurger.StoreCatalog.Repository;
using GeekBurger.StoreCatalog.Repository.Interfaces;
using Newtonsoft.Json;

namespace GeekBurger.StoreCatalog.Controllers
{
    [Route("api/product")]
    public class ProductsController : Controller
    {
        private readonly IApiService _apiService;
        private IMapper _mapper;
        private IProductsRepository _productRepository;
        private IProductionRepository _productionRepository;
        private IStoreRepository _storeRepository;
        private ILessOfferService _lessOfferService;
        private ILogService _logService;

        public ProductsController(IProductsRepository productRepository,
                                    IStoreRepository storeRepository,
                                    IApiService apiService,
                                    IMapper mapper,
                                    IProductionRepository productionRepository,
                                    ILessOfferService lessOfferService,
                                    ILogService logService)
        {
            _apiService = apiService;
            _mapper = mapper;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
            _productionRepository = productionRepository;
            _lessOfferService = lessOfferService;
        }

        /// <summary>
        /// Return all products for user with restrictions.
        /// </summary>
        /// <param name="storeName">Name of store</param>
        /// <param name="userId">Identify of user</param>
        /// <param name="restrictions">restrictions exaple: "soy|milk"</param>
        /// <response code="200">Returned Successfully</response>
        /// <response code="400">Returned Bad Request</response>
        /// <response code="404">Returned NoContent ingredients for consume.</response>
        /// <response code="500">Returned Inte ingredients for consume.</response>
        /// <returns></returns>
        [HttpGet("{storeName}/{userId}/{restrictions?}")]
        public IActionResult GetProduct(string storeName, Guid userId, List<string> restrictions)
        {
            Store store = _storeRepository.GetStoreByName(storeName);

            var actionResulValidation = Validate(storeName, userId, store);
            if (actionResulValidation != null)
                return actionResulValidation;

            if (restrictions == null || restrictions[0].Equals("{restrictions}"))
                restrictions = new List<string>();

            var restrictionTratado = string.Empty;
            if (restrictions.Count > 0)
                restrictionTratado = restrictions[0].Replace(",", "|");

            var restrictionList = restrictionTratado.Split("|").ToList();

            var user = new User() { UserId = userId, Restrictions = restrictionList };

            #region Request //GET api/production/areas request - Request sem parametro

            var areas = _productionRepository.GetAll();

            #endregion

            #region Request //GET api/products request (on StoreCatalog API)

            var products = _productRepository.GetAll();

            #endregion

            #region Request //GET api/products/byrestrictions request

            List<Ingredients.Contract.Response.IngredientsToUpsert> productsFilter = _apiService.GetProductsByRestrictions(store.StoreId, restrictionTratado).Result;

            if (productsFilter == null)
                return NoContent();

            #endregion

            #region Filtro de produtos por restrictions

            IEnumerable<Product> retorno = new List<Product>();

            try
            {
                var allowedAreas = areas.Where(area => user.Restrictions.All(restriction => area.Restrictions.Select(rest => rest.Restriction).Contains(restriction)));

                var allowedProducts = productsFilter.Where(product => product
                    .Ingredients.All(ingredient =>
                    allowedAreas.Any(area => !area.Restrictions.Select(rest => rest.Restriction).Contains(ingredient))));

                if (allowedProducts.Count() < 2)
                {
                    _lessOfferService.SendMessagesAsync(JsonConvert.SerializeObject(user));
                }

                retorno = products.Where(x => allowedProducts.Any(y => y.ProductId == x.ProductId));
            }
            catch (Exception ex)
            {
                _logService.SendMessagesAsync($"Error when try to filter products by restrictions. Message: {ex.Message}");
                return StatusCode(500);
            }

            #endregion

            if (retorno.Count() > 0)
                return Ok(retorno);
            else
                return NoContent();
        }

        /// <summary>
        /// Validações das chamadas evitando erros indesejados.
        /// </summary>
        /// <param name="storeName">Nome da Loja</param>
        /// <param name="userId">Identificador Único do usuário</param>
        /// <param name="store">Objeto da Loja que contem informações para validação</param>
        /// <returns>Em caso de Null está OK.</returns>
        private IActionResult Validate(string storeName, Guid userId,
            Store store)
        {
            if (string.IsNullOrWhiteSpace(storeName))
                return BadRequest("Field not informed storeName!");

            if (userId == Guid.Empty)
                return BadRequest("Field not informed userId!");

            if (store == null ||
                store.StoreId == Guid.Empty)
                return NotFound("Store not found!");

            return null;
        }
    }
}
