using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
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
        public ProductStandart Create(ProductStandart productStandart) { return _productStandartService.Create(productStandart); }
    }
}
