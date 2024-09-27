using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
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

        [HttpGet]
        public List<Product> Get()
        {
            return _productService.GetAllProdcuts();
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _productService.GetProductById(id);
        }

        [HttpGet("byStandart")]
        public List<Product> GetProductsByStandart(int id)
        {
            return _productService.GetProductsByStandart(id);
        }


        [HttpGet("byCategory/{id}")]
        public List<Product> GetProductsByCategory(int id)
        {
            return _productService.GetProductsByCategory(id);
        }


        [HttpGet("byStructure")]
        public List<Product> GetProductsByStructure(int id)
        {
            return _productService.GetProductsByStructure(id);
        }

        [HttpGet("byCertificate/{id}")]
        public List<Product> GetProductsByCertificate(int id)
        {
            return _productService.GetProductsByCertificate(id);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromForm] Product _product)
        {
            if (_product.Image == null || _product.Image.Length == 0)
                return BadRequest("No Image uploaded.");
            if (_product.DetailImage == null || _product.DetailImage.Length == 0)
                return BadRequest("No DetailImage uploaded.");
            byte[] imageBytes;
            byte[] detailImageBytes;

            using (var memoryStream = new MemoryStream())
            {
                await _product.Image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            using (var memoryStream = new MemoryStream())
            {
                await _product.DetailImage.CopyToAsync(memoryStream);
                detailImageBytes = memoryStream.ToArray();
            }
            var product = new Product
            {
                Type = _product.Type,
                UsageLocations = _product.UsageLocations,
                ImageData = imageBytes,
                CategoryId = _product.CategoryId,
                DetailImageData = detailImageBytes,
            };
            var createdProduct = _productService.CreateProduct(product);
            return Ok(createdProduct);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] Product updatedProduct)
        {
            if (updatedProduct.Id <= 0)
            {
                return BadRequest("Product ID is required.");
            }

            var existingProduct = _productService.GetProductById(updatedProduct.Id);

            if (existingProduct == null)
            {
                return NotFound($"Product with ID {updatedProduct.Id} not found.");
            }

      
            if (updatedProduct.Image != null && updatedProduct.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatedProduct.Image.CopyToAsync(memoryStream);
                    existingProduct.ImageData = memoryStream.ToArray();
                }
            }

            if (updatedProduct.DetailImage != null && updatedProduct.DetailImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatedProduct.DetailImage.CopyToAsync(memoryStream);
                    existingProduct.DetailImageData = memoryStream.ToArray();
                }
            }

            existingProduct.Type = string.IsNullOrEmpty(updatedProduct.Type) ? existingProduct.Type : updatedProduct.Type;
            existingProduct.UsageLocations = string.IsNullOrEmpty(updatedProduct.UsageLocations) ? existingProduct.UsageLocations : updatedProduct.UsageLocations;
            existingProduct.CategoryId = updatedProduct.CategoryId != 0 ? updatedProduct.CategoryId : existingProduct.CategoryId;

            var updatedProd = _productService.UpdateProduct(existingProduct);

            return Ok(updatedProd);
        }

        [HttpDelete("delete/{id}")]
        public void DeleteProduct(int id) { _productService.DeleteProduct(id); }
    }
}
