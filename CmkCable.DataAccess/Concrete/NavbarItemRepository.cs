using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class NavbarItemRepository : INavbarItemRepository
    {
        public NavbarItem CreateNavbarItem(NavbarItem navbarItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.NavbarItems.Add(navbarItem);
                cmkCableDbContext.SaveChanges();
                return navbarItem;
            }
        }

        public void DeleteNavbarItem(string id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedItem = cmkCableDbContext.NavbarItems.Find(id);
                cmkCableDbContext.NavbarItems.Remove(deletedItem);
                cmkCableDbContext.SaveChanges();
            }
        }


        public List<List<NavbarItemDto>> GetAllNavbarItems()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                // Tüm kayıtları veritabanından alıyoruz.
                var allTranslations = cmkCableDbContext.NavbarItemsTranslations
                    .OrderBy(x => x.NavbarItemId) // ID'ye göre sıralıyoruz.
                    .ToList();

                var groupedNavbarItems = new List<List<NavbarItemDto>>();

                // Mevcut NavbarItemId için geçici bir grup tutuyoruz.
                int? currentNavbarItemId = null;
                var currentGroup = new List<NavbarItemDto>();

                foreach (var item in allTranslations)
                {
                    if (currentNavbarItemId != item.NavbarItemId)
                    {
                        // Yeni bir grup oluşturma zamanıysa, önce mevcut grubu ekle.
                        if (currentGroup.Count > 0)
                        {
                            groupedNavbarItems.Add(currentGroup);
                        }

                        // Yeni gruba başlıyoruz.
                        currentNavbarItemId = item.NavbarItemId;
                        currentGroup = new List<NavbarItemDto>();
                    }

                    // Geçerli öğeyi gruba ekle.
                    currentGroup.Add(new NavbarItemDto
                    {
                        Id = item.NavbarItemId,
                        LanguageId = item.LanguageId,
                        Title = item.Title
                    });
                }

                // Son grubu ekle.
                if (currentGroup.Count > 0)
                {
                    groupedNavbarItems.Add(currentGroup);
                }

                return groupedNavbarItems;
            }
        }


        public NavbarItem GetNavbarItem(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.NavbarItems.Find(id);
            }
        }
        public List<NavbarItemDto> GetNavbarItemsByLanguage(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var navbarItems = cmkCableDbContext.NavbarItems
                    .Where(ni => ni.ParentId == null) // Ana öğeleri al
                    .Include(ni => ni.SubItems) // Alt öğeleri dahil et
                    .ThenInclude(sub => sub.Translations) // Alt öğe çevirilerini dahil et
                    .Include(ni => ni.Translations) // Ana öğe çevirilerini dahil et
                    .ToList();

                var navbarItemDtos = navbarItems.Select(item => new NavbarItemDto
                {
                    Id = item.Id,
                    Title = item.Translations.FirstOrDefault(t => t.LanguageId == languageId)?.Title,
                    Route = item.Route,
                    SubItems = item.SubItems.Select(subItem => new NavbarItemDto
                    {
                        Id = subItem.Id,
                        Title = subItem.Translations.FirstOrDefault(t => t.LanguageId == languageId)?.Title,
                        Route = subItem.Route
                    }).ToList()
                }).ToList();

                return navbarItemDtos; 
            }
        }





        public List<NavbarItemDto> UpdateNavbarItem(List<NavbarItemDto> navbarItems)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                foreach (var navbarItem in navbarItems)
                {
                    var item = cmkCableDbContext.NavbarItemsTranslations
                                    .FirstOrDefault(n => n.NavbarItemId == navbarItem.Id && n.LanguageId == navbarItem.LanguageId);

                    if (item == null)
                    {
                        item = new NavbarItemTranslation
                        {
                            NavbarItemId = navbarItem.Id,
                            LanguageId = navbarItem.LanguageId,
                            Title = navbarItem.Title
                        };
                        cmkCableDbContext.NavbarItemsTranslations.Add(item);
                    }
                    else
                    {
                        item.Title = navbarItem.Title;
                    }
                }

                cmkCableDbContext.SaveChanges();
            }

            return navbarItems;
        }

    }
}
