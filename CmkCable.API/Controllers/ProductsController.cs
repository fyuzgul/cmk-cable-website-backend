using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Net;
using DTOs.CreateDTOs;
using DTOs.UpdateDTOs;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authorization;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private IProductService _productService;
        private CloudinaryManager _cloudinaryManager;

        public ProductsController()
        {

            _productService = new ProductManager();
            _cloudinaryManager = new CloudinaryManager();

        }

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
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto, [FromForm] List<string> translations, [FromForm] List<int> languageIds)
        {
            string imageUrl = null;
            string detailImageUrl = null;
            if (productDto.Image != null && productDto.Image.Length > 0)
            {

                imageUrl = await _cloudinaryManager.UploadImage(productDto.Image,"product-covers");
            }

            if (productDto.DetailImage != null && productDto.DetailImage.Length > 0)
            {
                detailImageUrl = await _cloudinaryManager.UploadImage(productDto.DetailImage, "product-details");
            }

            var product = new Product
            {
                Type = productDto.Type,
                Image = imageUrl,
                DetailImage = detailImageUrl,
                CategoryId = productDto.CategoryId
            };

            var createdProduct = await _productService.CreateProduct(product, translations, languageIds);

            return CreatedAtAction(nameof(CreateProduct), new { id = createdProduct.Id }, createdProduct);
        }




        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _productService.GetProductById(id, 1);
            if (product == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            var deletionResult = await _cloudinaryManager.DestoryImage(product.Image);
            var deletionResultDetail = await _cloudinaryManager.DestoryImage(product.DetailImage);

            if (deletionResult.Result.Equals("ok") && deletionResultDetail.Result.Equals("ok"))
            {
                _productService.DeleteProduct(id);
                return NoContent();
            }
            else
            {
                Console.WriteLine($"Cloudinary hata: {deletionResult.Error?.Message}");
                return BadRequest($"Resim silinirken bir hata oluştu: {deletionResult.Error?.Message}");
            }
        }





        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDTO _product,
    [FromForm] List<string> translations,
    [FromForm] List<int> languageIds)
        {
            string imageUrl = null;
            string detailImageUrl = null;

            var product = _productService.GetProductById(_product.Id, 1);
            if (product == null || product.Id <= 0)
                return BadRequest("Invalid product ID.");

            if ((_product.Image) != null && _product.Image.Length > 0)
            {
                DeletionResult imageDeletionResult = await _cloudinaryManager.DestoryImage(product.Image);
                if (imageDeletionResult.Result.Equals("ok"))
                {
                  imageUrl = await _cloudinaryManager.UploadImage(_product.Image, "product-covers");
                }
            }

            if ((_product.DetailImage) != null)
            {
                DeletionResult detailImageDeletionResult = await _cloudinaryManager.DestoryImage(product.DetailImage);
                if (detailImageDeletionResult.Result.Equals("ok"))
                {
                    detailImageUrl = await _cloudinaryManager.UploadImage(_product.DetailImage, "product-details");
                }

            }

            var lastProduct = new Product
            {
                Id = _product.Id,
                Type = _product.Type,
                Image = imageUrl ?? product.Image,
                DetailImage = detailImageUrl ?? product.DetailImage,
                CategoryId = _product.CategoryId
            };

            var updatedProduct = await _productService.UpdateProduct(lastProduct, translations, languageIds);

            if (updatedProduct == null)
                return NotFound("Product not found.");

            return Ok(updatedProduct);
        }




    }
}
