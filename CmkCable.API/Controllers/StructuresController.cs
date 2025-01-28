using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StructuresController : ControllerBase
    {
        private IStructureService _structureService;
        public StructuresController() { _structureService = new StructureManager(); }

        [HttpGet]
        public List<StructureDTO> GetAllStructures() { return _structureService.GetAllStructures(); }

        [HttpGet("{id}")]
        public StructureDTO GetStructureById(int id) { return _structureService.GetStructureById(id); }

        [HttpGet("byProduct/{id}")]
        public List<Structure> GetStructuresByProductId(int id) { return _structureService.GetStructuresByProductId(id); }

        [HttpPost("create")]
        [Authorize]
        public Task<Structure> CreateStructure([FromForm]Structure structure, [FromForm] List<string> translations, [FromForm] List<int> languageIds) { return _structureService.CreateStructure(structure, translations, languageIds); }

        [HttpPut("update")]
        [Authorize]
        public Structure UpdateStructure([FromForm] Structure structure, [FromForm] List<string> translations, [FromForm] List<int> languageIds) { return _structureService.UpdateStructure(structure, translations, languageIds); }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public void DeleteStructure(int id) { _structureService.DeleteStructure(id); }

        [HttpGet("byLanguage/{languageId}")]
        public List<StructureDTO> GetStructuresByLanguageId(int languageId) { return _structureService.GetStructuresByLanguageId(languageId); }

    }
}
