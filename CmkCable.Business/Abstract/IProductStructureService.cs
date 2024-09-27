using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IProductStructureService
    {
        public List<ProductStructure> GettAllStructuresByProductId(int id);

    }
}
