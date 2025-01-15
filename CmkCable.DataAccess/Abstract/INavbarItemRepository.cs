using CmkCable.Entities;
using System.Collections.Generic;
using DTOs;

namespace CmkCable.DataAccess.Abstract
{
    public interface INavbarItemRepository
    {
        List<List<NavbarItemDto>> GetAllNavbarItems();
        NavbarItem GetNavbarItem(int id);
        NavbarItem CreateNavbarItem(NavbarItem navbarItem);
        List<NavbarItemDto> GetNavbarItemsByLanguage(int languageId);
        List<NavbarItemDto> UpdateNavbarItem(List<NavbarItemDto> navbarItems);
        void DeleteNavbarItem(string id);
    }
}
