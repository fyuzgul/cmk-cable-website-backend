using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdsDocumentsController : ControllerBase
    {
        private IPdsDocumentService _documentService;
        public PdsDocumentsController() { _documentService = new PdsDocumentManager(); }

        [HttpGet("{languageId}")]
        public List<PdsDocumentDTO> GetAllPdsDocumentsWithLanguage(int languageId) { return _documentService.GetAllPdsDocumentsWithLanguage(languageId); }

    }
}
