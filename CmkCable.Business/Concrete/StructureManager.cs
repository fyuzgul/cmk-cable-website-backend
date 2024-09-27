using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class StructureManager : IStructureService
    {
        private IStructureRepository _structureRepository;
        public StructureManager() { _structureRepository = new StructureRepository(); }

        public Structure CreateStructure(Structure structure)
        {
            return _structureRepository.CreateStructure(structure);
        }

        public void DeleteStructure(int id)
        {
            _structureRepository.DeleteStructure(id);
        }

        public List<Structure> GetAllStructures()
        {
            return _structureRepository.GetAllStructures(); 
        }

        public Structure GetStructureById(int id)
        {
            return _structureRepository.GetStructureById(id);
        }

        public List<Structure> GetStructuresByProductId(int id)
        {
            return _structureRepository.GetStructuresByProductId(id);
        }

        public Structure UpdateStructure(Structure structure)
        {
            return _structureRepository.UpdateStructure(structure);
        }
    }
}
