using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Model;
using GeekBurger.StoreCatalog.Repository;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GeekBurger.StoreCatalog.Controllers
{
    public class StoreController : Controller
    {
        private IStoreRepository _storeRepository;

        public StoreController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        /// <summary>
        /// Check if store is available.
        /// </summary>
        /// <response code="200">Returned successfully</response>
        /// <response code="404">Returned when store is not load</response>
        /// <response code="503">Returned services not available</response>        
        [HttpGet]
        [Route("api/store")]
        public IActionResult Store()
        {
            try
            {
                Store store = _storeRepository.GetStore();
                StoreCatalogReady storeCatalogReady;

                if (store != null)
                {
                    storeCatalogReady = new StoreCatalogReady();
                    storeCatalogReady.Ready = true;
                    storeCatalogReady.StoreId = store.StoreId;
                    return Ok(storeCatalogReady);
                }
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(503);
            }
        }
    }
}
