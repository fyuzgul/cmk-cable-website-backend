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

        public List<NavbarItem> GetAllNavbarItems()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.NavbarItems.ToList();
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
                var navbarItemsWithTranslations = from ni in cmkCableDbContext.NavbarItems
                                                  join nit in cmkCableDbContext.NavbarItemsTranslations
                                                  on ni.Id equals nit.NavbarItemId
                                                  where nit.LanguageId == languageId
                                                  select new
                                                  {
                                                      ni.Id,
                                                      nit.Title,
                                                      ni.Route
                                                  };

                var navbarItemDtos = navbarItemsWithTranslations.Select(item => new NavbarItemDto
                {
                    Id = item.Id,
                    Title = item.Title,
                    Route = item.Route
                }).ToList();

                return navbarItemDtos;
            }
        }


        public NavbarItem UpdateNavbarItem(NavbarItem navbarItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.NavbarItems.Update(navbarItem);
                cmkCableDbContext.SaveChanges();
                return navbarItem;
            }
        }

    }
}
