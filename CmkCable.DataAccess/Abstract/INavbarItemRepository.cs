using CmkCable.Entities;
using System.Collections.Generic;
using DTOs;

namespace CmkCable.DataAccess.Abstract
{
    public interface INavbarItemRepository
    {
        List<NavbarItem> GetAllNavbarItems();
        NavbarItem GetNavbarItem(int id);
        NavbarItem CreateNavbarItem(NavbarItem navbarItem);
        List<NavbarItemDto> GetNavbarItemsByLanguage(int languageId);
        NavbarItem UpdateNavbarItem(NavbarItem navbarItem);
        void DeleteNavbarItem(string id);
    }
}
