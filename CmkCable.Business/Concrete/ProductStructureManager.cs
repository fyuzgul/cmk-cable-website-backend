using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class ProductStructureManager : IProductStructureService
    {
        private IProductStructureRepository _productStructureRepository;
        public ProductStructureManager()
        {
            _productStructureRepository = new ProductStructureRepository();
        }
        public ProductStructure CreateProductStructure(ProductStructure productStructure)
        {
            return _productStructureRepository.CreateProductStructure(productStructure);
        }

        public void DeletProductStructure(int productId, int structureId)
        {
             _productStructureRepository.DeletProductStructure(productId, structureId);
        }

        public List<ProductStructure> GetAllProductStructures(int ProductId)
        {
           return _productStructureRepository.GetAllProductStructures(ProductId);
        }

        public ProductStructure UpdateProductStructure(ProductStructure productStructure)
        {
            return _productStructureRepository.UpdateProductStructure(productStructure);
        }
    }
}
