using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductStandartRepository : IProductStandartRepository
    {
        public ProductStandart Create(ProductStandart productStandart)
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) 
            {
                cmkCableDbContext.ProductStandarts.Add(productStandart);
                cmkCableDbContext.SaveChanges();
                return productStandart;
            }
        }
    }
}
