using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private ILanguageService _languageService;
        public LanguagesController()
        {
            _languageService = new LanguageManager();
        }

        [HttpGet]
        public List<Language> GetLanguages() { return _languageService.GetLanguages(); }

        [HttpDelete("{id}")]
        [Authorize]
        public void DeleteLanguage(int id) { _languageService.DeleteLanguage(id); }
        [HttpPost("create")]
        [Authorize]
        public Language CreateLanguage([FromBody] Language language) { return _languageService.CreateLanguage(language); }
        [HttpPut("update")]
        [Authorize]
        public Language UpdateLanguage([FromBody] Language language) { return _languageService.UpdateLanguage(language); }
    }
}
