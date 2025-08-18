using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using DTOs.UpdateDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageTextsController : ControllerBase
    {
        private IHomePageTextService _homePageTextService;
        public HomePageTextsController() { _homePageTextService = new HomePageTextManager(); }

        [HttpGet("{name}/{languageId}")]
        public HomePageTextDTO GetHomePageTextByName(string name, int languageId) { return _homePageTextService.GetHomePageTextByName(name, languageId); }

        [HttpGet("id/{id}/{languageId}")]
        public HomePageTextDTO GetHomePageTextById(int id, int languageId) { return _homePageTextService.GetHomePageTextById(id, languageId); }

        [HttpGet("{languageId}")]
        public List<HomePageTextDTO> GetHomeAllPageTexts(int languageId) { return _homePageTextService.GetHomeAllPageTexts(languageId); }

        [HttpGet]
        public List<HomePageTextDTO> GetHomePageTextsWithAllTranslations() { return _homePageTextService.GetHomePageTextsWithAllTranslations(); }

        [HttpPost]
        [Authorize]
        public HomePageTextDTO CreateHomePageText(CreateHomePageTextWithTranslationsDTO createDto) 
        { 
            return _homePageTextService.CreateHomePageText(createDto); 
        }

        [HttpDelete("{id}")]
        [Authorize]
        public bool DeleteHomePageText(int id) 
        { 
            return _homePageTextService.DeleteHomePageText(id); 
        }

        [HttpPut("update")]
        [Authorize]
        public void UpdateHomeText(List<HomePageTextUpdateDTO> homePageTextUpdateDTOs) { _homePageTextService.UpdateHomeText(homePageTextUpdateDTOs); }
    }
}
