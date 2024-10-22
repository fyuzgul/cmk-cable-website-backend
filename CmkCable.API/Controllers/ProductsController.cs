using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        public ProductsController() { _productService = new ProductManager(); }

        [HttpGet("all/{languageId}")]
        public List<ProductDTO> Get(int languageId)
        {
            return _productService.GetAllProducts(languageId);
        }

        [HttpGet("{id}")]
        public ProductDTO GetProductWithAllTranslations(int id) { return _productService.GetProductWithAllTranslations(id); }
        [HttpGet("{id}/{languageId}")]
        public ProductDTO GetProductById(int id, int languageId) { return _productService.GetProductById(id, languageId); }

        [HttpGet("byStandart")]
        public List<ProductDTO> GetProductsByStandart(int id)
        {
            return _productService.GetProductsByStandart(id);
        }


        [HttpGet("byCategory/{id}")]
        public List<ProductDTO> GetProductsByCategory(int id)
        {
            return _productService.GetProductsByCategory(id);
        }

        [HttpGet("byCertificate/{id}")]
        public List<ProductDTO> GetProductsByCertificate(int id)
        {
            return _productService.GetProductsByCertificate(id);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] Product _product, [FromForm] List<string> translations, [FromForm] List<int> languageIds)
        {
            


            var product = new Product
            {
                Type = _product.Type,
                Image = _product.Image,
                CategoryId = _product.CategoryId,
                DetailImage = _product.DetailImage,
            };

            var createdProduct = await _productService.CreateProduct(product, translations, languageIds);

            return CreatedAtAction(nameof(CreateProduct), new { id = createdProduct.Id }, createdProduct);
        }



        [HttpDelete("delete/{id}")]
        public void DeleteProduct(int id) { _productService.DeleteProduct(id); }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] Product product,
     [FromForm] List<string> translations,
     [FromForm] List<int> languageIds)
        {
            if (product.Id <= 0)
                return BadRequest("Invalid product ID.");

            var updatedProduct = await _productService.UpdateProduct(product, translations, languageIds);

            if (updatedProduct == null)
                return NotFound("Product not found.");

            return Ok(updatedProduct);
        }

    }
}
