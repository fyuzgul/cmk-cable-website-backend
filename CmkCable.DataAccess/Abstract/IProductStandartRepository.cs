using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IProductStandartRepository
    {
        ProductStandart Create(ProductStandart productStandart);
        void Delete(int productId, int standartId); 
    }
}
