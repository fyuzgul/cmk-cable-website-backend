using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStandartsController : ControllerBase
    {
        private IProductStandartService _productStandartService;
        public ProductStandartsController()
        {
            _productStandartService = new ProductStandartManager();
        }

        [HttpPost("create")]
        [Authorize]
        public ProductStandart Create(ProductStandart productStandart) { return _productStandartService.Create(productStandart); }

        [HttpDelete("delete/{productId}/{standartId}")]
        [Authorize]
        public void Delete(int productId, int standartId) { _productStandartService.Delete(productId, standartId); }
    }
}
