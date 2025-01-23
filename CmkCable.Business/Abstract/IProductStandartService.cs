using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IProductStandartService
    {
        ProductStandart Create(ProductStandart productStandart);
        void Delete(int productId, int standartId); 
    }
}
