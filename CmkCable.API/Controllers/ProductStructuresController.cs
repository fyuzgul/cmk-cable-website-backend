using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductStructuresController : ControllerBase
    {
        private IProductStructureService _productStructureService;
        public ProductStructuresController()
        {
            _productStructureService = new ProductStructureManager();
        }

        [HttpPost("create")]
        public ProductStructure Create(ProductStructure productStructure) { return _productStructureService.CreateProductStructure(productStructure); }

        [HttpDelete("delete/{productId}/{structureId}")]
        public void Delete(int productId, int structureId) { _productStructureService.DeletProductStructure(productId, structureId); }

        [HttpGet("byProduct/{productId}")]
        public List<ProductStructure> GetAll(int productId) { return _productStructureService.GetAllProductStructures(productId); }

    }
}
