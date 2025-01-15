using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class LanguageManager : ILanguageService
    {
        private ILanguageRepository _languageRepository;
        public LanguageManager()
        {
            _languageRepository = new LanguageRepository();
        }

        public Language CreateLanguage(Language language)
        {
            return _languageRepository.CreateLanguage(language);
        }

        public void DeleteLanguage(int id)
        {
            _languageRepository.DeleteLanguage(id); 
        }

        public List<Language> GetLanguages()
        {
            return _languageRepository.GetLanguages();
        }

        public Language UpdateLanguage(Language language)
        {
           return _languageRepository.UpdateLanguage(language); 
        }
    }
}
