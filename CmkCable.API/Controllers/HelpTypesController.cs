using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpTypesController : ControllerBase
    {
        private IHelpTypeService _helpTypeService;

        public HelpTypesController()
        {
            _helpTypeService = new HelpTypeManager();
        }

        [HttpGet]
        public IActionResult GetAllHelpTypes()
        {
            var helpTypes = _helpTypeService.GetAllHelpTypes();
            return Ok(helpTypes);
        }

        [HttpGet("active")]
        public IActionResult GetActiveHelpTypes()
        {
            var helpTypes = _helpTypeService.GetActiveHelpTypes();
            return Ok(helpTypes);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetHelpTypeById(int id)
        {
            var helpType = _helpTypeService.GetHelpTypeById(id);
            if (helpType == null)
                return NotFound();
            return Ok(helpType);
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult CreateHelpType([FromBody] HelpType helpType)
        {
            if (helpType == null)
                return BadRequest();

            var createdHelpType = _helpTypeService.CreateHelpType(helpType);
            return CreatedAtAction(nameof(GetHelpTypeById), new { id = createdHelpType.Id }, createdHelpType);
        }

        [HttpPost("create-with-translations")]
        [Authorize]
        public IActionResult CreateHelpTypeWithTranslations([FromBody] DTOs.CreateDTOs.CreateHelpTypeWithTranslationsDTO request)
        {
            if (request == null)
                return BadRequest();

            var helpType = new HelpType
            {
                Name = request.Name,
                IsActive = request.IsActive
            };

            var translations = new List<HelpTypeTranslation>();
            if (request.Translations != null)
            {
                foreach (var t in request.Translations)
                {
                    translations.Add(new HelpTypeTranslation
                    {
                        LanguageId = t.LanguageId,
                        Name = t.Name
                    });
                }
            }

            var created = _helpTypeService.CreateHelpTypeWithTranslations(helpType, translations);
            return CreatedAtAction(nameof(GetHelpTypeById), new { id = created.Id }, created);
        }

        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateHelpType([FromBody] HelpType helpType)
        {
            if (helpType == null || helpType.Id <= 0)
                return BadRequest();

            var updatedHelpType = _helpTypeService.UpdateHelpType(helpType);
            if (updatedHelpType == null)
                return NotFound();

            return Ok(updatedHelpType);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult DeleteHelpType(int id)
        {
            if (id <= 0)
                return BadRequest();

            _helpTypeService.DeleteHelpType(id);
            return NoContent();
        }
    }
}

