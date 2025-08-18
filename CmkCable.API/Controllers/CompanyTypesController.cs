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
    public class CompanyTypesController : ControllerBase
    {
        private ICompanyTypeService _companyTypeService;

        public CompanyTypesController()
        {
            _companyTypeService = new CompanyTypeManager();
        }

        [HttpGet]
        public IActionResult GetAllCompanyTypes()
        {
            var companyTypes = _companyTypeService.GetAllCompanyTypes();
            return Ok(companyTypes);
        }

        [HttpGet("active")]
        public IActionResult GetActiveCompanyTypes()
        {
            var companyTypes = _companyTypeService.GetActiveCompanyTypes();
            return Ok(companyTypes);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetCompanyTypeById(int id)
        {
            var companyType = _companyTypeService.GetCompanyTypeById(id);
            if (companyType == null)
                return NotFound();
            return Ok(companyType);
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult CreateCompanyType([FromBody] CompanyType companyType)
        {
            if (companyType == null)
                return BadRequest();

            var createdCompanyType = _companyTypeService.CreateCompanyType(companyType);
            return CreatedAtAction(nameof(GetCompanyTypeById), new { id = createdCompanyType.Id }, createdCompanyType);
        }

        [HttpPost("create-with-translations")]
        [Authorize]
        public IActionResult CreateCompanyTypeWithTranslations([FromBody] DTOs.CreateDTOs.CreateCompanyTypeWithTranslationsDTO request)
        {
            if (request == null)
                return BadRequest();

            var companyType = new CompanyType
            {
                Name = request.Name,
                IsActive = request.IsActive
            };

            var translations = new List<CompanyTypeTranslation>();
            if (request.Translations != null)
            {
                foreach (var t in request.Translations)
                {
                    translations.Add(new CompanyTypeTranslation
                    {
                        LanguageId = t.LanguageId,
                        Name = t.Name
                    });
                }
            }

            var created = _companyTypeService.CreateCompanyTypeWithTranslations(companyType, translations);
            return CreatedAtAction(nameof(GetCompanyTypeById), new { id = created.Id }, created);
        }

        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateCompanyType([FromBody] CompanyType companyType)
        {
            if (companyType == null || companyType.Id <= 0)
                return BadRequest();

            var updatedCompanyType = _companyTypeService.UpdateCompanyType(companyType);
            if (updatedCompanyType == null)
                return NotFound();

            return Ok(updatedCompanyType);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult DeleteCompanyType(int id)
        {
            if (id <= 0)
                return BadRequest();

            _companyTypeService.DeleteCompanyType(id);
            return NoContent();
        }
    }
}

