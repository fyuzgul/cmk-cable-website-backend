using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class LanguageRepository : ILanguageRepository
    {
        public List<Language> GetLanguages()
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) 
            {
                return cmkCableDbContext.Languages.ToList();    
            }

        }
    }
}
