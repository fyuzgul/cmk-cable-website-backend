using CloudinaryDotNet.Actions;
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
using System.IO;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateTypesController : ControllerBase
    {
        private ICertificateTypeService _certificateTypeService;
        private CloudinaryManager _cloudinaryManager;
        public CertificateTypesController()
        {
            _certificateTypeService = new CertificateTypeManager();
            _cloudinaryManager = new CloudinaryManager();
        }

        [HttpGet]
        public List<CertificateTypeDTO> GetAllCertificateTypes()
        {
            return _certificateTypeService.GetAllCertificateTypes();
        }

        [HttpGet("{id}")]
        public CertificateType GetCertificateTypeById(int id)
        {
            return _certificateTypeService.GetCertificateTypeById(id);
        }
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateCertificateType([FromForm] CreateCertificateTypeDTO _certificateType)
        {
            string imageUrl = await _cloudinaryManager.UploadImage(_certificateType.Image, "document-types");

            var certificateType = new CertificateType
            {
                Name = _certificateType.Name.ToUpper(),
                Image = imageUrl
            };

            var createdCertificateType = _certificateTypeService.CreateCertificateType(certificateType);

            return Ok(createdCertificateType);
        }


        [HttpDelete("delete/{id}")]
        [Authorize]
        public void DeleteCertificateType(int id) { _certificateTypeService.DeleteCertificateType(id); }


        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCertificateType([FromForm] UpdateCertifcateTypeDTO updatedCertificateType)
        {
            string imageUrl = null;
            if (updatedCertificateType.Id <= 0)
            {
                return BadRequest("Product ID is required.");
            }

            var existingCertificateType = _certificateTypeService.GetCertificateTypeById(updatedCertificateType.Id);

            if (existingCertificateType == null)
            {
                return NotFound($"Certficate type with ID {updatedCertificateType.Id} not found.");
            }


            if (updatedCertificateType.Image != null && updatedCertificateType.Image.Length > 0)
            {
                DeletionResult deletionResult = await _cloudinaryManager.DestoryImage(existingCertificateType.Image);
                if (deletionResult.Result.Equals("ok"))
                {
                    imageUrl = await _cloudinaryManager.UploadImage(updatedCertificateType.Image, "document-types");
                }
            }
            else
            {
                imageUrl = existingCertificateType.Image;
            }

            var certificateType = new CertificateType
            {
                Id = updatedCertificateType.Id,
                Name = updatedCertificateType.Name,
                Image = imageUrl
            };
            
            var updatedCertType = _certificateTypeService.UpdateCertificateType(certificateType);

            return Ok(updatedCertType);
        }
    }
}
