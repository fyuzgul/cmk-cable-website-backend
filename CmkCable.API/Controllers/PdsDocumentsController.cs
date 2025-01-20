using CloudinaryDotNet.Actions;
using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using DTOs.UpdateDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdsDocumentsController : ControllerBase
    {
        private  IPdsDocumentService _documentService;
        private  CloudinaryManager _cloudinaryManager;

        public PdsDocumentsController()
        {
            _documentService = new PdsDocumentManager();
            _cloudinaryManager = new CloudinaryManager();
        }

        [HttpGet("bySelectedLanguage/{languageId}")]
        public List<PdsDocumentDTO> GetAllPdsDocumentsWithLanguage(int languageId)
        {
            return _documentService.GetAllPdsDocumentsWithLanguage(languageId);
        }

        [HttpGet]
        public List<PdsDocumentDTO> GetAll()
        {
            return _documentService.GetAll();
        }

        [HttpGet("{id:int}")]
        public ActionResult<PdsDocumentDTO> GetPdsDocumentWithAllTranslations(int id)
        {
            var document = _documentService.PdsDocumentWithAllTranslations(id);
            if (document == null)
            {
                return NotFound();
            }
            return document;
        }

        [HttpPost]
        public async Task<ActionResult<PdsDocument>> CreatePdsDocument([FromForm] CreatePdsDocumentDTO createPdsDocument, [FromForm] List<string> translations, [FromForm] List<int> languageIds)
        {
            string imageUrl = "https://res.cloudinary.com/dk7nt7ar5/image/upload/v1730807470/7a1d0281-977f-4be1-a12a-3fcace994705_oih48s.jpg";
            string pdfUrl = null;
            pdfUrl = await _cloudinaryManager.UploadPdf(createPdsDocument.FileContent, "pds-document-images");


            var pdsDocument = new PdsDocumentDTO
            {
                Image = imageUrl,
                FileContent = pdfUrl,   
            };
            


            var createdDocument = await _documentService.CreatePdsDocument(pdsDocument, translations, languageIds);
            return Ok(createdDocument);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdatePdsDocument([FromForm] UpdatePdsDocumentDTO pdsDocumentDTO, [FromForm] List<string> translations, [FromForm] List<int> languageIds)
        {
            string pdfUrl = null;
            var document = _documentService.PdsDocumentWithAllTranslations(pdsDocumentDTO.Id);
            if (document == null)
                return BadRequest("Invalid document ID.");
            if (pdsDocumentDTO.FileContent != null && pdsDocumentDTO.FileContent.Length > 0) 
            {
                DeletionResult deletionResult = await _cloudinaryManager.DestroyPdf(document.FileContent);
                if (deletionResult.Result.Equals("ok"))
                {
                    pdfUrl = await _cloudinaryManager.UploadPdf(pdsDocumentDTO.FileContent, "pds-document-pdfs");
                }
            }
            else
            {
                pdfUrl = document.FileContent;
            }

            var lastDocument = new PdsDocument
            {
                Id = pdsDocumentDTO.Id,
                FileContent = pdfUrl,
                Image = document.Image
            };
            var updated = await _documentService.UpdatePdsDocument(lastDocument, translations, languageIds);
            return Ok(updated);    

        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeletePdsDocument(int id)
        {
            _documentService.DeletePdsDocument(id);
            return NoContent();
        }
    }
}
