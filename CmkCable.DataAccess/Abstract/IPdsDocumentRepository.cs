using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface IPdsDocumentRepository
    {
        List<PdsDocumentDTO> GetAllPdsDocumentsWithLanguage(int languageId);
        public List<PdsDocumentDTO> GetAll();
        PdsDocumentDTO PdsDocumentWithAllTranslations(int pdsId);
        Task<PdsDocument> CreatePdsDocument(PdsDocumentDTO pdsDocumentDTO, List<string> translations, List<int> languageIds);
        Task<PdsDocument> UpdateCategory(PdsDocument pdsDocument, List<string> translations, List<int> languageIds);
        void DeletePdsDocument(int id);

    }
}
