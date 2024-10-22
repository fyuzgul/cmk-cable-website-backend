using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface INavbarItemService
    {
        List<NavbarItem> GetAllNavbarItems();
        NavbarItem GetNavbarItem(int id);
        NavbarItem CreateNavbarItem(NavbarItem navbarItem);
        List<NavbarItemDto> GetNavbarItemsByLanguage(int languageId);
        NavbarItem UpdateNavbarItem(NavbarItem navbarItem);
        void DeleteNavbarItem(string id);
    }
}
