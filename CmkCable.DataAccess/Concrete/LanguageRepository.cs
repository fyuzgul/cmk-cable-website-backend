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
        public Language CreateLanguage(Language language)
        {
           using(var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Languages.Add(language);
                cmkCableDbContext.SaveChanges();
                return language;
            }   
        }

        public void DeleteLanguage(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var relatedItems = cmkCableDbContext.AboutUsItems
                                                    .Where(x => x.LanguageId == id)
                                                    .ToList();

                foreach (var item in relatedItems)
                {
                    item.LanguageId = 1;
                }

                cmkCableDbContext.SaveChanges();

                var language = cmkCableDbContext.Languages.Find(id);
                if (language != null)
                {
                    cmkCableDbContext.Languages.Remove(language);
                    cmkCableDbContext.SaveChanges();
                }
            }
        }


        public List<Language> GetLanguages()
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) 
            {
                return cmkCableDbContext.Languages.ToList();    
            }

        }

        public Language UpdateLanguage(Language language)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Languages.Update(language);
                cmkCableDbContext.SaveChanges();
                return language;
            }
        }
    }
}
