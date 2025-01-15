using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Concrete
{
    public class StructureManager : IStructureService
    {
        private IStructureRepository _structureRepository;
        public StructureManager() { _structureRepository = new StructureRepository(); }

        public Task<Structure> CreateStructure(Structure structure, List<string> translations, List<int> languageIds)
        {
            return _structureRepository.CreateStructure(structure, translations, languageIds);
        }

        public void DeleteStructure(int id)
        {
            _structureRepository.DeleteStructure(id);
        }

        public List<StructureDTO> GetAllStructures()
        {
            return _structureRepository.GetAllStructures(); 
        }

        public StructureDTO GetStructureById(int id)
        {
            return _structureRepository.GetStructureById(id);
        }

        public List<StructureDTO> GetStructuresByLanguageId(int languageId)
        {
            return _structureRepository.GetStructuresByLanguageId(languageId);
        }

        public List<Structure> GetStructuresByProductId(int id)
        {
            return _structureRepository.GetStructuresByProductId(id);
        }

        public Structure UpdateStructure(Structure structure, List<string> translations, List<int> languageIds)
        {
            return _structureRepository.UpdateStructure(structure, translations, languageIds);
        }
    }
}
