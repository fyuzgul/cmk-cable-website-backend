using CmkCable.Business.Abstract;
using CmkCable.DataAccess;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;

        public ProductManager() { _productRepository = new ProductRepository(); }

        public async Task<Product> CreateProduct(Product product, List<string> translations, List<int> languageIds)
        {
            return await _productRepository.CreateProduct(product, translations, languageIds);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
        }

        public List<ProductDTO> GetAllProducts(int languageId)
        {
            return _productRepository.GetAllProducts(languageId);
        }

        public ProductDTO GetProductWithAllTranslations(int id)
        {
            return _productRepository.GetProductWithAllTranslations(id);
        }

        public List<ProductDTO> GetProductsByCategory(int categoryId)
        {
            return _productRepository.GetProductsByCategory(categoryId);
        }

        public List<ProductDTO> GetProductsByCertificate(int certificateId)
        {
            return _productRepository.GetProductsByCertificate(certificateId);
        }

        public List<ProductDTO> GetProductsByStandart(int standartId)
        {
            return _productRepository.GetProductsByStandart(standartId);
        }
        public Task<Product> UpdateProduct(Product product, List<string> translations, List<int> languageIds)
        {
            return _productRepository.UpdateProduct(product, translations, languageIds);
        }

        public ProductDTO GetProductById(int id, int languageId)
        {
            return _productRepository.GetProductById(id, languageId);
        }
    }
}
