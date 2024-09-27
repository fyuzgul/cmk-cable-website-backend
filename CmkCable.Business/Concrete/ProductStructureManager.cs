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

        public ProductStructureManager() { _productStructureRepository = new ProductStructureRepository(); }
        public List<ProductStructure> GettAllStructuresByProductId(int id)
        {
            return _productStructureRepository.GettAllStructuresByProductId(id);
        }
    }
}
