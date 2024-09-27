using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IProductService
    {
        List<Product> GetAllProdcuts();
        Product GetProductById(int id);
        List<Product> GetProductsByCategory(int categoryId);
        List<Product> GetProductsByStandart(int standartId);
        List<Product> GetProductsByStructure(int structureId);
        List<Product> GetProductsByCertificate(int certificateId);
        Product CreateProduct(Product product);
        Product UpdateProduct(Product product);
        void DeleteProduct(int id);
    }
}
