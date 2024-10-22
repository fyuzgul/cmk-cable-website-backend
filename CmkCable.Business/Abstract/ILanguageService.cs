using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface ILanguageService
    {
        List<Language> GetLanguages();
    }
}
