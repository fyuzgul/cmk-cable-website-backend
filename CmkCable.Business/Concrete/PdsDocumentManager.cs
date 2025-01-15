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
    public class PdsDocumentManager : IPdsDocumentService
    {
        private IPdsDocumentRepository _repository;
        public PdsDocumentManager() { _repository = new PdsDocumentRepository(); }

        public Task<PdsDocument> CreatePdsDocument(PdsDocumentDTO pdsDocumentDTO, List<string> translations, List<int> languageIds)
        {
            throw new NotImplementedException();
        }

        public void DeletePdsDocument(int id)
        {
            throw new NotImplementedException();
        }

        public List<PdsDocumentDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<PdsDocumentDTO> GetAllPdsDocumentsWithLanguage(int languageId)
        {
            return _repository.GetAllPdsDocumentsWithLanguage(languageId);    
        }

        public PdsDocumentDTO PdsDocumentWithAllTranslations(int pdsId)
        {
            throw new NotImplementedException();
        }

        public Task<PdsDocument> UpdateCategory(PdsDocument pdsDocument, List<string> translations, List<int> languageIds)
        {
            throw new NotImplementedException();
        }
    }
}
