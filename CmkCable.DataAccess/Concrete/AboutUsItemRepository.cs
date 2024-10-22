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

        public void DeleteAboutUsItem(string id)
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
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.AboutUsItems.ToList();
            }
        }

        public AboutUsItem UpdateAboutUsItem(AboutUsItem aboutUsItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.AboutUsItems.Update(aboutUsItem); 
                return aboutUsItem; 
            }
        }
    }
}
