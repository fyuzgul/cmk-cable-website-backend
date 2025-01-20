using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class SwiperItemRepository : ISwiperItemRepository
    {
        public SwiperItem CreateSwiperItem(SwiperItem swiperItem)
        {
            using (var context = new CmkCableDbContext())
            {
                context.SwiperItems.Add(swiperItem);
                context.SaveChanges();
                return swiperItem;
            }
        }

        public void DeleteSwiperItem(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                var deletedItem = context.SwiperItems.Find(id);
                context.SwiperItems.Remove(deletedItem);
                context.SaveChanges();
            }
        }

        public List<SwiperItem> GetAllSwiperItems()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.SwiperItems.ToList();
            }
        }

        public SwiperItem GetSwiperItemById(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                return context.SwiperItems.Find(id);
            }
        }

        public SwiperItem UpdateSwiperItem(SwiperItem swiperItem)
        {
            using (var context = new CmkCableDbContext())
            {
                var existingItem = context.SwiperItems
                                          .FirstOrDefault(x => x.Id == swiperItem.Id); // Veritabanından var olan öğeyi bul

                if (existingItem != null)
                {
                    existingItem.Image = swiperItem.Image; // Yeni değerlerle mevcut öğeyi güncelle
                    context.SaveChanges(); // Değişiklikleri kaydet
                }

                return existingItem; // Güncellenmiş öğeyi geri döndür
            }
        }


    }
}
