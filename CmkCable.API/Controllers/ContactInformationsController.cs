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
    public class ContactInformationsController : ControllerBase
    {
        private IContactInformationService _contactInformationService;
        public ContactInformationsController()
        {
            _contactInformationService = new ContactInfromationManager();
        }

        [HttpGet("bylanguage/{languageId}")]
        public List<ContactInformationDTO> GetAllContactInformations(int languageId) { return _contactInformationService.GetAllContactInformations(languageId);}

        [HttpPost("create")]
        [Authorize]
        public ContactInformation CreateContactInformation(ContactInformation contactInformation) { return _contactInformationService.CreateContactInformation(contactInformation); }

        [HttpGet("{id}")]
        public ContactInformation GetContactInformation(int id) { return _contactInformationService.GetContactInformation(id); }
        [HttpDelete("delete")]
        [Authorize]
        public void DeleteContactInformation(int id) {_contactInformationService.DeleteContactInformation(id);}
        [HttpPut("update")]
        [Authorize]
        public ContactInformation UpdateContactInformation(ContactInformation contactInformation) { return _contactInformationService.UpdateContactInformation(contactInformation); }


    }
}
