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
        public ActionResult<List<ContactInformationDTO>> GetAllContactInformations(int languageId)
        {
            return Ok(_contactInformationService.GetAllContactInformations(languageId));
        }

        [HttpPost]
        [Authorize]
        public ActionResult<ContactInformationDetailDTO> CreateContactInformation([FromBody] ContactInformationCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _contactInformationService.CreateContactInformation(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public ActionResult<ContactInformationDetailDTO> GetContactInformation(int id)
        {
            var result = _contactInformationService.GetContactInformation(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteContactInformation(int id)
        {
            var existing = _contactInformationService.GetContactInformation(id);
            if (existing == null)
                return NotFound();

            _contactInformationService.DeleteContactInformation(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<ContactInformationDetailDTO> UpdateContactInformation(int id, [FromBody] ContactInformationCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _contactInformationService.GetContactInformation(id);
            if (existing == null)
                return NotFound();

            var result = _contactInformationService.UpdateContactInformation(dto, id);
            return Ok(result);
        }

        [HttpGet("all")]
        public ActionResult<List<ContactInformationDetailDTO>> GetAllContactInformationsWithTranslations()
        {
            var result = _contactInformationService.GetAllContactInformationsWithTranslations();
            return Ok(result);
        }
    }
}
