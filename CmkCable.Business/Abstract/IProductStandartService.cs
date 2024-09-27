using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IProductStandartService
    {
        public List<ProductStandart> GetAllStandartsByProductId(int productId);
        public void DeleteProductStandart(int id);

        public ProductStandart CreateProductStandart(ProductStandart productStandart);


    }
}
