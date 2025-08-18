using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormsController : ControllerBase
    {
        private IContactRequestService _contactRequestService;
        private IRoleService _roleService;
        private ICompanyTypeService _companyTypeService;
        private IHelpTypeService _helpTypeService;

        public ContactFormsController()
        {
            _contactRequestService = new ContactRequestManager();
        }

        [HttpPost("create")]
        public IActionResult Post([FromBody] CreateContactRequestDTO createContactRequestDTO)
        {
            if (createContactRequestDTO == null)
                return BadRequest("Contact request data is required.");

            // Map DTO to entity
            var contactRequest = new ContactRequest
            {
                FullName = createContactRequestDTO.FullName,
                Street = createContactRequestDTO.Street,
                City = createContactRequestDTO.City,
                Postcode = createContactRequestDTO.Postcode,
                TelephoneNumber = createContactRequestDTO.TelephoneNumber,
                Email = createContactRequestDTO.Email,
                Message = createContactRequestDTO.Message,
                IpAddress = createContactRequestDTO.IpAddress,
                Consent = createContactRequestDTO.Consent,
                CreatedAt = DateTime.UtcNow
            };

            _contactRequestService.CreateContactRequest(contactRequest);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllContactRequests()
        {
            var contactRequests = _contactRequestService.GetAllContactRequests();
            
            // Map to DTOs
            var contactRequestDTOs = contactRequests.Select(cr => new ContactRequestDTO
            {
                Id = cr.Id,
                FullName = cr.FullName,
                Street = cr.Street,
                City = cr.City,
                Postcode = cr.Postcode,
                TelephoneNumber = cr.TelephoneNumber,
                Email = cr.Email,
                Message = cr.Message,
                IpAddress = cr.IpAddress,
                Consent = cr.Consent,
                CreatedAt = cr.CreatedAt
            }).ToList();

            return Ok(contactRequestDTOs);
        }

        [HttpGet("get/{id}")]
        public IActionResult Get(int id)
        {
            var contactRequest = _contactRequestService.GetContactRequestById(id);
            if (contactRequest == null)
                return NotFound();

            var contactRequestDTO = new ContactRequestDTO
            {
                Id = contactRequest.Id,
                FullName = contactRequest.FullName,
                Street = contactRequest.Street,
                City = contactRequest.City,
                Postcode = contactRequest.Postcode,
                TelephoneNumber = contactRequest.TelephoneNumber,
                Email = contactRequest.Email,
                Message = contactRequest.Message,
                IpAddress = contactRequest.IpAddress,
                Consent = contactRequest.Consent,
                CreatedAt = contactRequest.CreatedAt
            };

            return Ok(contactRequestDTO);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _contactRequestService.DeleteContactRequest(id);
            return Ok();
        }


    }
}
