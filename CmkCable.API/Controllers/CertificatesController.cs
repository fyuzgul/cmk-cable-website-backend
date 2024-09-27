using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private ICertificateService _certificateService;

        public CertificatesController()
        {
            _certificateService = new CertificateManager();
        }

        [HttpGet]
        public List<Certificate> GetAllCertificates() { return _certificateService.GetAllCertifacets(); }

        [HttpGet("byType/{id}")]
        public List<Certificate> GetAllCertificatesByTypeId(int id) { return _certificateService.GetAllCertificatesByTypeId(id); }

        [HttpGet("{id}")]
        public Certificate GetCertifacetById(int id) { return _certificateService.GetCertifacetById(id); }

        [HttpDelete("{id}")]
        public void DeleteCertificate(int id) { _certificateService.DeleteCertificate(id); }

        [HttpPost("create")]
        public async Task<IActionResult> UploadCertificate([FromForm] Certificate _certificate)
        {
            if (_certificate.Image == null || _certificate.Image.Length == 0)
                return BadRequest("No Image uploaded.");
            if (_certificate.FileContent == null || _certificate.FileContent.Length == 0)
                return BadRequest("No file uploaded.");

            byte[] photoBytes;
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await _certificate.Image.CopyToAsync(memoryStream);
                photoBytes = memoryStream.ToArray();
            }
            using (var memoryStream = new MemoryStream())
            {
                await _certificate.FileContent.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var certificate = new Certificate
            {
                Name = _certificate.Name,
                Id = _certificate.Id,
                ImageData = photoBytes,
                FileContentData = fileBytes,
                ProductCertificates = _certificate.ProductCertificates,
                TypeId = _certificate.TypeId
            };

            var createdCertificate = _certificateService.CreateCertificate(certificate);

            return Ok(createdCertificate);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCertificate([FromForm] Certificate updatedCertificate)
        {
            if (updatedCertificate.Id <= 0)
            {
                return BadRequest("Certificate ID is required.");
            }

            var existingCertificate = _certificateService.GetCertifacetById(updatedCertificate.Id);
            if (existingCertificate == null)
            {
                return NotFound($"Certificate with ID {updatedCertificate.Id} not found.");
            }

            if (updatedCertificate.Image != null && updatedCertificate.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatedCertificate.Image.CopyToAsync(memoryStream);
                    existingCertificate.ImageData = memoryStream.ToArray();
                }
            }

            if (updatedCertificate.FileContent != null && updatedCertificate.FileContent.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updatedCertificate.FileContent.CopyToAsync(memoryStream);
                    existingCertificate.FileContentData = memoryStream.ToArray();
                }
            }

            existingCertificate.Name = string.IsNullOrEmpty(updatedCertificate.Name) ? existingCertificate.Name : updatedCertificate.Name;
            existingCertificate.TypeId = updatedCertificate.TypeId != 0 ? updatedCertificate.TypeId : existingCertificate.TypeId;


            var updatedCert = _certificateService.UpdateCertificate(existingCertificate);

            return Ok(updatedCert);
        }


    }
}
