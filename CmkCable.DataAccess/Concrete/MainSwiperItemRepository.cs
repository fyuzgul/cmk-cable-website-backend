using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class MainSwiperItemRepository : IMainPageSwiperItemRepository
    {
        public MainPageSwiperItem CreateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.MainPageSwiperItems.Add(mainPageSwiperItem);
                cmkCableDbContext.SaveChanges();
                return mainPageSwiperItem;
            }
        }

        public void DeleteMainPageSwiperItem(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedItem = cmkCableDbContext.MainPageSwiperItems.Find(id);
                cmkCableDbContext.MainPageSwiperItems.Remove(deletedItem);
                cmkCableDbContext.SaveChanges() ;
            }
        }

        public List<MainPageSwiperItem> GetAlMainPageSwiperItems()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
               return cmkCableDbContext.MainPageSwiperItems.ToList();   
            }
        }

        public MainPageSwiperItem GetMainPageSwiperItemById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
               return cmkCableDbContext.MainPageSwiperItems.Find(id);
            }
        }

        public MainPageSwiperItem UpdateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.MainPageSwiperItems.Update(mainPageSwiperItem);
                cmkCableDbContext.SaveChanges();
                return mainPageSwiperItem;
            }
        }
    }
}
