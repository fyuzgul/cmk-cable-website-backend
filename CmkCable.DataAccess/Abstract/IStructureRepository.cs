using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IStructureRepository
    {
        List<Structure> GetAllStructures();
        Structure GetStructureById(int id);
        List<Structure> GetStructuresByProductId(int prodcutId);
        Structure CreateStructure(Structure structure);
        Structure UpdateStructure (Structure structure);
        void DeleteStructure(int id);
    }
}
