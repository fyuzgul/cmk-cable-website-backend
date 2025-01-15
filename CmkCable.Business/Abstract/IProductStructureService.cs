using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IProductStructureService
    {
        ProductStructure CreateProductStructure(ProductStructure productStructure);
        ProductStructure UpdateProductStructure(ProductStructure productStructure);
        void DeletProductStructure(int productId, int structureId);
        List<ProductStructure> GetAllProductStructures(int ProductId);
    }
}
