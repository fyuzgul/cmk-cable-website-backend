using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavbarItemsController : ControllerBase
    {
        private INavbarItemService navbarItemService;
        public NavbarItemsController()
        {
            navbarItemService = new NavbarItemManager();    
        }

        [HttpGet]
        public List<List<NavbarItemDto>> GetAllNavbarItems() { return navbarItemService.GetAllNavbarItems();}

        [HttpGet("{languageId}")]
        public IActionResult GetNavbarItems(int languageId)
        {
            var navbarItems = navbarItemService.GetNavbarItemsByLanguage(languageId);
            return Ok(navbarItems);
        }

        [HttpPost("create")]
        [Authorize]
        public NavbarItem CreateNavbarItem(NavbarItem navbarItem) {return navbarItemService.CreateNavbarItem(navbarItem);}

        [HttpPut("update")]
        [Authorize]
        public List<NavbarItemDto> UpdateNavbarItem(List<NavbarItemDto> navbarItems) { return navbarItemService.UpdateNavbarItem(navbarItems); }
    }
}
