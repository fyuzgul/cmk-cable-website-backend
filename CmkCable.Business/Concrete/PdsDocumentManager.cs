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

       

        public Task<PdsDocument> CreatePdsDocument(PdsDocumentDTO pdsDocument, List<string> translations, List<int> languageIds)
        {
            return _repository.CreatePdsDocument(pdsDocument, translations, languageIds);
        }

        public void DeletePdsDocument(int id)
        {
            _repository.DeletePdsDocument(id);
        }

        public List<PdsDocumentDTO> GetAll()
        {
            return _repository.GetAll();
        }

        public List<PdsDocumentDTO> GetAllPdsDocumentsWithLanguage(int languageId)
        {
            return _repository.GetAllPdsDocumentsWithLanguage(languageId);    
        }

        public PdsDocumentDTO PdsDocumentWithAllTranslations(int pdsId)
        {
            return _repository.PdsDocumentWithAllTranslations(pdsId);
        }

        public Task<PdsDocument> UpdatePdsDocument(PdsDocument pdsDocument, List<string> translations, List<int> languageIds)
        {
            return _repository.UpdatePdsDocument(pdsDocument, translations, languageIds);
        }
    }
}
