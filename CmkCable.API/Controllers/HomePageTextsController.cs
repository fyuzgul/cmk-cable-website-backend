using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.UpdateDTOs;
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

        [HttpGet("{languageId}")]
        public List<HomePageTextDTO> GetHomeAllPageTexts(int languageId) { return _homePageTextService.GetHomeAllPageTexts(languageId); }

        [HttpGet]
        public List<HomePageTextDTO> GetHomePageTextsWithAllTranslations() { return _homePageTextService.GetHomePageTextsWithAllTranslations(); }

        [HttpPut("update")]
        public void UpdateHomeText(List<HomePageTextUpdateDTO> homePageTextUpdateDTOs) { _homePageTextService.UpdateHomeText(homePageTextUpdateDTOs); }
    }
}
