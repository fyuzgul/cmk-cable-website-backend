﻿using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface ILanguageRepository
    {
        List<Language> GetLanguages();  
        Language CreateLanguage(Language language);
        Language UpdateLanguage(Language language);
        void DeleteLanguage(int id);
    }
}
