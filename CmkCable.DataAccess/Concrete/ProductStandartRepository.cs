using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


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

        public void Delete(int productId, int standartId)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                var productStandart = cmkCableDbContext.ProductStandarts.FirstOrDefault(ps => ps.ProductId == productId && ps.StandartId == standartId);
                if (productStandart != null)
                {
                    cmkCableDbContext.ProductStandarts.Remove(productStandart);
                    cmkCableDbContext.SaveChanges();
                }   
            }
        }
    }
}
