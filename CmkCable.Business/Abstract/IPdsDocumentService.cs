using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Abstract
{
    public interface IPdsDocumentService
    {
        List<PdsDocumentDTO> GetAllPdsDocumentsWithLanguage(int languageId);
         List<PdsDocumentDTO> GetAll();
        PdsDocumentDTO PdsDocumentWithAllTranslations(int pdsId);
        Task<PdsDocument> CreatePdsDocument(PdsDocumentDTO pdsDocument, List<string> translations, List<int> languageIds);
        Task<PdsDocument> UpdatePdsDocument(PdsDocument pdsDocument, List<string> translations, List<int> languageIds);
        void DeletePdsDocument(int id);
    }
}
