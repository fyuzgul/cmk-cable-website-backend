using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface IStructureRepository
    {
        List<StructureDTO> GetAllStructures();
        StructureDTO GetStructureById(int id);
        List<Structure> GetStructuresByProductId(int prodcutId);
        Task<Structure> CreateStructure(Structure structure, List<string> translations, List<int> languageIds);
        Structure UpdateStructure (Structure structure, List<string> translations, List<int> languageIds);
        List<StructureDTO> GetStructuresByLanguageId(int languageId);
        void DeleteStructure(int id);
    }
}
