using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface IProductRepository
    {
        List<ProductDTO> GetAllProducts(int languageId);
        ProductDTO GetProductWithAllTranslations(int id);
        List<ProductDTO> GetProductsByCategory(int categoryId);
        ProductDTO GetProductById(int id, int languageId);
        List<ProductDTO> GetProductsByStandart(int standartId);
        List<ProductDTO> GetProductsByCertificate(int certificateId);
        Task<Product> CreateProduct(Product product, List<string> translations, List<int> languageIds);
        Task<Product> UpdateProduct(Product product, List<string> translations, List<int> languageIds);
        void DeleteProduct(int id);



    }
}
