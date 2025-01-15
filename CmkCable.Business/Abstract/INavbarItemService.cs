using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface INavbarItemService
    {
        List<List<NavbarItemDto>> GetAllNavbarItems();
        NavbarItem GetNavbarItem(int id);
        NavbarItem CreateNavbarItem(NavbarItem navbarItem);
        List<NavbarItemDto> GetNavbarItemsByLanguage(int languageId);
        List<NavbarItemDto> UpdateNavbarItem(List<NavbarItemDto> navbarItems);
        void DeleteNavbarItem(string id);
    }
}
