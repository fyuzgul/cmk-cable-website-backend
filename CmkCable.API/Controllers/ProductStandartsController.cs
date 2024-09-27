using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStandartsController : ControllerBase
    {
        private IProductStandartService _productStandartsService;

        public ProductStandartsController() { _productStandartsService = new ProductStandartManager(); }

        [HttpGet("{id}")]
        public List<ProductStandart> Get(int id) { return _productStandartsService.GetAllStandartsByProductId(id); }

        [HttpDelete("delete/{id}")]
        public void Delete(int id) { _productStandartsService.DeleteProductStandart(id); }

        [HttpPost("create")]
        public ProductStandart Create(ProductStandart productStandart) {return  _productStandartsService.CreateProductStandart(productStandart); }

    }
}
