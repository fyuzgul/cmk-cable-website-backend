using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductStandartRepository : IProductStandartRepository
    {
        public ProductStandart CreateProductStandart(ProductStandart productStandart)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.ProductStandarts.Add(productStandart);
                cmkCableDbContext.SaveChanges();
                return productStandart; 
            }
        }

        public void DeleteProductStandart(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedRecord = cmkCableDbContext.ProductStandarts.Find(id);
                cmkCableDbContext.ProductStandarts.Remove(deletedRecord);
                cmkCableDbContext.SaveChanges() ;
            }

        }

        public List<ProductStandart> GetAllStandartsByProductId(int productId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) { 
                return cmkCableDbContext.ProductStandarts.Where(p => p.ProductId == productId).ToList();
            }
        }
    }
}
