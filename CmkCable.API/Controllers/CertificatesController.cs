using CloudinaryDotNet.Actions;
using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.DataAccess;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using DTOs.UpdateDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public IActionResult GetAllCertificates()
        {
            using (var context = new CmkCableDbContext())
            {
                var certificates = context.Certificates
                    .Select(c => new CertificateDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        FileContent = c.FileContent,
                        Image = c.Image,
                        DopNumber = c.DopNumber,
                        CertificateType = new CertificateTypeDTO
                        {
                            Id = c.TypeId,
                            Name = context.CertificateTypes
                                .Where(ct => ct.Id == c.TypeId)
                                .Select(ct => ct.Name)
                                .FirstOrDefault()
                        },
                        ProductNames = context.ProductCertificates
                            .Where(pc => pc.CertificateId == c.Id)
                            .Join(context.Products,
                                pc => pc.ProductId,
                                p => p.Id,
                                (pc, p) => p.Type)
                            .ToList()
                    })
                    .ToList();

                return Ok(certificates);
            }
        }

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
                TypeId = _certificate.TypeId,
                DopNumber = _certificate.DopNumber
            };

            var createdCertificate = _certificateService.CreateCertificate(certificate);

            return Ok(createdCertificate);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCertificate([FromForm] UpdateCertificateDTO updatedCertificate)
        {
            try
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

                string imageUrl = existingCertificate.Image;
                string pdfUrl = existingCertificate.FileContent;

                if (updatedCertificate.Image != null && updatedCertificate.Image.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingCertificate.Image))
                    {
                        DeletionResult deletionResult = await _cloudinaryManager.DestoryImage(existingCertificate.Image);
                        if (deletionResult.Result.Equals("ok"))
                        {
                            imageUrl = await _cloudinaryManager.UploadImage(updatedCertificate.Image, "document-image");
                        }
                    }
                    else
                    {
                        imageUrl = await _cloudinaryManager.UploadImage(updatedCertificate.Image, "document-image");
                    }
                }

                if (updatedCertificate.FileContent != null && updatedCertificate.FileContent.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingCertificate.FileContent))
                    {
                        DeletionResult deletion = await _cloudinaryManager.DestroyPdf(existingCertificate.FileContent);
                        if (deletion.Result.Equals("ok"))
                        {
                            pdfUrl = await _cloudinaryManager.UploadPdf(updatedCertificate.FileContent, "document-pdfs");
                        }
                    }
                    else
                    {
                        pdfUrl = await _cloudinaryManager.UploadPdf(updatedCertificate.FileContent, "document-pdfs");
                    }
                }

                var certificate = new Certificate
                {
                    Id = updatedCertificate.Id,
                    Name = updatedCertificate.Name ?? existingCertificate.Name,
                    TypeId = updatedCertificate.TypeId,
                    Image = imageUrl,
                    FileContent = pdfUrl,
                    //deneme
                    DopNumber = updatedCertificate.DopNumber  
                };

                var updatedCert = _certificateService.UpdateCertificate(certificate);
                return Ok(updatedCert);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("search/dop")]
        public IActionResult SearchByDopNumber([FromQuery] string dopNumber)
        {
            if (string.IsNullOrWhiteSpace(dopNumber))
                return BadRequest("DOP number is required.");

            using (var context = new CmkCableDbContext())
            {
                var certificates = context.Certificates
                    .Where(c => c.DopNumber != null && c.DopNumber.Contains(dopNumber))
                    .Select(c => new CertificateDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        FileContent = c.FileContent,
                        Image = c.Image,
                        DopNumber = c.DopNumber,
                        CertificateType = new CertificateTypeDTO
                        {
                            Id = c.TypeId,
                            Name = context.CertificateTypes
                                .Where(ct => ct.Id == c.TypeId)
                                .Select(ct => ct.Name)
                                .FirstOrDefault()
                        },
                        ProductNames = context.ProductCertificates
                            .Where(pc => pc.CertificateId == c.Id)
                            .Join(context.Products,
                                pc => pc.ProductId,
                                p => p.Id,
                                (pc, p) => p.Type)
                            .ToList()
                    })
                    .ToList();

                return Ok(certificates);
            }
        }
    }
}
