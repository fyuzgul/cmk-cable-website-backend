using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class AboutUsItemRepository : IAboutUsItemRepository
    {
        public AboutUsItem CreateAboutUsItem(AboutUsItem aboutUsItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.AboutUsItems.Add(aboutUsItem);
                cmkCableDbContext.SaveChanges();
                return aboutUsItem;
            }
        }

        public void DeleteAboutUsItem(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedItem = cmkCableDbContext.AboutUsItems.Find(id);
                cmkCableDbContext.AboutUsItems.Remove(deletedItem);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<AboutUsItem> GetAllAboutUsItems()
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.AboutUsItems.ToList();
            }   
        }

        public List<AboutUsItem> GetAllAboutUsItemsWithLanguage(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                // Ana dildeki sonuçları al
                var items = cmkCableDbContext.AboutUsItems
                    .Where(item => item.LanguageId == languageId)
                    .ToList();

                // Eğer ana dilde sonuç yoksa alternatif dilde ara
                if (!items.Any())
                {
                    items = cmkCableDbContext.AboutUsItems
                        .Where(item => item.LanguageId == 2) // Alternatif dil ID'si
                        .ToList();
                }

                return items;
            }
        }


        public AboutUsItem UpdateAboutUsItem(int id, AboutUsItem updatedAboutUsItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var existingItem = cmkCableDbContext.AboutUsItems.Find(id);

                if (existingItem == null)
                {
                    return null;
                }

                updatedAboutUsItem.Id = existingItem.Id;

                cmkCableDbContext.Entry(existingItem).CurrentValues.SetValues(updatedAboutUsItem);
                cmkCableDbContext.SaveChanges();

                return existingItem;
            }
        }




    }
}
