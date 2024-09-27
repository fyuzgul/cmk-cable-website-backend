using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
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
        public CertificateTypesController()
        {
            _certificateTypeService = new CertificateTypeManager();
        }

        [HttpGet]
        public List<CertificateType> GetAllCertificateTypes()
        {
            return _certificateTypeService.GetAllCertificateTypes();
        }

        [HttpGet("{id}")]
        public CertificateType GetCertificateTypeById(int id)
        {
            return _certificateTypeService.GetCertificateTypeById(id);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCertificateType([FromForm] CertificateType _certificateType)
        {
            if (_certificateType.Image == null || _certificateType.Image.Length == 0)
                return BadRequest("No Image uploaded.");
            byte[] imageBytes;

            using (var memoryStream = new MemoryStream())
            {
                await _certificateType.Image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            var certificateType = new CertificateType
            {
                Name = _certificateType.Name,
                ImageData = imageBytes,
            };
            var createdCertificateType = _certificateTypeService.CreateCertificateType(certificateType);    
            return Ok(createdCertificateType);
        }

        [HttpDelete("delete/{id}")]
        public void DeleteCertificateType(int id) { _certificateTypeService.DeleteCertificateType(id); }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateCertificateType([FromForm] CertificateType updatedCertificateType)
        {
            if (updatedCertificateType.Id <= 0)
            {
                return BadRequest("Product ID is required.");
            }

            var existingCertificateType = _certificateTypeService.GetCertificateTypeById(updatedCertificateType.Id);

            if (existingCertificateType == null)
            {
                return NotFound($"Product with ID {updatedCertificateType.Id} not found.");
            }


            if (updatedCertificateType.Image != null && updatedCertificateType.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatedCertificateType.Image.CopyToAsync(memoryStream);
                    existingCertificateType.ImageData = memoryStream.ToArray();
                }
            }

            existingCertificateType.Name= string.IsNullOrEmpty(updatedCertificateType.Name) ? existingCertificateType.Name : updatedCertificateType.Name;
            
            var updatedCertType = _certificateTypeService.UpdateCertificateType(existingCertificateType);

            return Ok(updatedCertType);
        }
    }
}
