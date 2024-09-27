using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IProductStandartRepository
    {
        public List<ProductStandart> GetAllStandartsByProductId(int productId);
        ProductStandart CreateProductStandart(ProductStandart productStandart);
        void DeleteProductStandart(int id);
    }
}
