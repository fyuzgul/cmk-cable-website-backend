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
        public List<Language> GetLanguages()
        {
            return _languageRepository.GetLanguages();
        }
    }
}
