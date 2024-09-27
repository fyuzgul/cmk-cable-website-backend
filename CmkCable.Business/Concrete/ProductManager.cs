using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        public ProductManager() { _productRepository = new ProductRepository(); }

        public Product CreateProduct(Product product)
        {
            return _productRepository.CreateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
        }

        public List<Product> GetAllProdcuts()
        {
            return _productRepository.GetAllProdcuts();
        }

        public Product GetProductById(int id)
        {
            return _productRepository.GetProductById(id);
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _productRepository.GetProductsByCategory(categoryId);
        }

        public List<Product> GetProductsByCertificate(int certificateId)
        {
            return _productRepository.GetProductsByCertificate(certificateId);
        }

        public List<Product> GetProductsByStandart(int standartId)
        {
            return _productRepository.GetProductsByStandart(standartId);  
        }

        public List<Product> GetProductsByStructure(int structureId)
        {
            return _productRepository.GetProductsByStructure(structureId);
        }

        public Product UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }
    }
}
