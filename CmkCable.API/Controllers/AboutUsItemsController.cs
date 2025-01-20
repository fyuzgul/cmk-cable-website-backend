using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutUsItemsController : ControllerBase
    {
        private IAboutUsItemService _aboutUsItemService;

        public AboutUsItemsController()
        {
            _aboutUsItemService = new AboutUsItemManager();
        }
        [HttpGet]
        public List<AboutUsItem> GetAllAboutUsItems() { return _aboutUsItemService.GetAllAboutUsItems(); }

        [HttpGet("{languageId}")]
        public List<AboutUsItem> GetAllAboutUsItemsWithLanguage(int languageId) { return _aboutUsItemService.GetAllAboutUsItemsWithLanguage(languageId); }

        [HttpPost("create")]
        public AboutUsItem CreateAboutUsItem(AboutUsItem aboutUsItem) {return _aboutUsItemService.CreateAboutUsItem(aboutUsItem);}

        [HttpPut("update/{id}")]
        public AboutUsItem UpdateAboutUsItem(int id, AboutUsItem aboutUsItem) { return _aboutUsItemService.UpdateAboutUsItem(id, aboutUsItem); }

        [HttpDelete("delete/{id}")]
        public void DeleteAboutUsItem(int id) { _aboutUsItemService.DeleteAboutUsItem(id); }    

    }
}
