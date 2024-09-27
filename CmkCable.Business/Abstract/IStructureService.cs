using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IStructureService
    {

        List<Structure> GetAllStructures();
        Structure GetStructureById(int id);
        List<Structure> GetStructuresByProductId(int id);
        Structure CreateStructure(Structure structure);
        Structure UpdateStructure(Structure structure);
        void DeleteStructure(int id);

    }
}
