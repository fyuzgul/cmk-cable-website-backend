using CloudinaryDotNet.Actions;
using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using DTOs.UpdateDTOs;
using Microsoft.AspNetCore.Authorization;
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
        private ICertificateTypeService _certificateTypeService;    
        private CloudinaryManager _cloudinaryManager;   

        public CertificatesController()
        {
            _certificateService = new CertificateManager();
            _certificateTypeService = new CertificateTypeManager();
            _cloudinaryManager = new CloudinaryManager();
        }

        [HttpGet]
        public List<CertificateDTO> GetAllCertificates() { return _certificateService.GetAllCertifacets(); }

        [HttpGet("byType/{id}")]
        public List<Certificate> GetAllCertificatesByTypeId(int id) { return _certificateService.GetAllCertificatesByTypeId(id); }

        [HttpGet("{id}")]
        public Certificate GetCertifacetById(int id) { return _certificateService.GetCertifacetById(id); }

        [HttpDelete("{id}")]
        [Authorize]
        public void DeleteCertificate(int id) { _certificateService.DeleteCertificate(id); }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> UploadCertificate([FromForm] CreateCertificateDTO _certificate)
        {
            string typeName = _certificateTypeService.GetCertificateTypeById(_certificate.TypeId).Name;
            

            if (_certificate.Image == null || _certificate.Image.Length == 0)
                return BadRequest("No Image uploaded.");
            if (_certificate.FileContent == null || _certificate.FileContent.Length == 0)
                return BadRequest("No file uploaded.");

            string imageUrl = await _cloudinaryManager.UploadImage(_certificate.Image, "document-images/" + typeName);
            string pdfUrl = await _cloudinaryManager.UploadPdf(_certificate.FileContent, "document-pdfs/" + typeName);
            var certificate = new Certificate
            {
                Name = _certificate.Name,
                Image = imageUrl,
                FileContent = pdfUrl,
                TypeId = _certificate.TypeId
            };

            var createdCertificate = _certificateService.CreateCertificate(certificate);

            return Ok(createdCertificate);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCertificate([FromForm] UpdateCertificateDTO updatedCertificate)
        {
            string imageUrl = null, pdfUrl = null;
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
                DeletionResult deletionResult = await _cloudinaryManager.DestoryImage(existingCertificate.Image);
                if (deletionResult.Result.Equals("ok"))
                {
                    imageUrl = await _cloudinaryManager.UploadImage(updatedCertificate.Image, "document-image");
                }
            }

            if (updatedCertificate.FileContent != null && updatedCertificate.FileContent.Length > 0)
            {
                DeletionResult deletion = await _cloudinaryManager.DestroyPdf(existingCertificate.FileContent);
                if (deletion.Result.Equals("ok"))
                {
                    pdfUrl = await _cloudinaryManager.UploadPdf(updatedCertificate.FileContent, "document-pdfs");
                }
            }

            var certificate = new Certificate
            {
                Name = updatedCertificate.Name,
                TypeId = updatedCertificate.Id,
                Image = imageUrl,
                FileContent = pdfUrl,
            };


            var updatedCert = _certificateService.UpdateCertificate(certificate);

            return Ok(updatedCert);
        }


    }
}
