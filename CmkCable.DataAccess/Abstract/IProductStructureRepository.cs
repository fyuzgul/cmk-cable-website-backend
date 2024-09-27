using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IProductStructureRepository
    {
        public List<ProductStructure> GettAllStructuresByProductId(int id);
    }
}
