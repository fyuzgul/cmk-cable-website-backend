using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class ProductStandartManager : IProductStandartService
    {
        private IProductStandartRepository _productStandartRepository;
        public ProductStandartManager() { _productStandartRepository = new ProductStandartRepository(); }

        public ProductStandart CreateProductStandart(ProductStandart productStandart)
        {
            return _productStandartRepository.CreateProductStandart(productStandart);
        }

        public void DeleteProductStandart(int id)
        {
            _productStandartRepository.DeleteProductStandart(id);
        }

        public List<ProductStandart> GetAllStandartsByProductId(int productId)
        {
           return _productStandartRepository.GetAllStandartsByProductId(productId);
        }
    }
}
