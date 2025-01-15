using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductStructureRepository : IProductStructureRepository
    {
        public ProductStructure CreateProductStructure(ProductStructure productStructure)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.ProductStructures.Add(productStructure);
                cmkCableDbContext.SaveChanges();
                return productStructure;
            }
        }

        public void DeletProductStructure(int productId, int structureId)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                var productStructure = cmkCableDbContext.ProductStructures.FirstOrDefault(ps => ps.ProductId == productId && ps.StructureId == structureId);
                if (productStructure != null)
                {
                    cmkCableDbContext.ProductStructures.Remove(productStructure);
                    cmkCableDbContext.SaveChanges();
                }
            }
        }

        public List<ProductStructure> GetAllProductStructures(int ProductId)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.ProductStructures.Where(ps => ps.ProductId == ProductId).ToList();
            }
        }

     
        public ProductStructure UpdateProductStructure(ProductStructure productStructure)
        {
           using(var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.ProductStructures.Update(productStructure);
                cmkCableDbContext.SaveChanges();
                return productStructure;
            }
        }
    }
}
