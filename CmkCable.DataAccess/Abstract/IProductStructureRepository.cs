using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface IProductStructureRepository
    {
        ProductStructure CreateProductStructure(ProductStructure productStructure);
        ProductStructure UpdateProductStructure(ProductStructure productStructure);
        void DeletProductStructure(int productId, int structureId);
        List<ProductStructure> GetAllProductStructures(int ProductId);
    }
}
