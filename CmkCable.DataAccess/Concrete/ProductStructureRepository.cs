using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductStructureRepository : IProductStructureRepository
    {
        public List<ProductStructure> GettAllStructuresByProductId(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) { 
                return cmkCableDbContext.ProductStructures.Where(predicate => predicate.ProductId == id).ToList();
            }
        }
    }
}
