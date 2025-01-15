using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Abstract
{
    public interface IStructureService
    {

        List<StructureDTO> GetAllStructures();
        StructureDTO GetStructureById(int id);
        List<Structure> GetStructuresByProductId(int id);
        Task<Structure> CreateStructure(Structure structure, List<string> translations, List<int> languageIds);
        Structure UpdateStructure(Structure structure, List<string> translations, List<int> languageIds );
        void DeleteStructure(int id);
        List<StructureDTO> GetStructuresByLanguageId(int languageId);   

    }
}
