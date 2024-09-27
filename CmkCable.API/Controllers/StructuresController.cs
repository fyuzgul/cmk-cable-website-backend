using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StructuresController : ControllerBase
    {
        private IStructureService _structureService;
        public StructuresController() { _structureService = new StructureManager(); }

        [HttpGet]
        public List<Structure> GetAllStructures() { return _structureService.GetAllStructures(); }

        [HttpGet("{id}")]
        public Structure GetStructureById(int id) { return _structureService.GetStructureById(id); }

        [HttpGet("byProduct/{id}")]
        public List<Structure> GetStructuresByProductId(int id) { return _structureService.GetStructuresByProductId(id); }

        [HttpPost("create")]
        public Structure CreateStructure(Structure structure) { return _structureService.CreateStructure(structure); }

        [HttpPut("update")]
        public Structure UpdateStructure(Structure structure) { return _structureService.UpdateStructure(structure); }

        [HttpDelete("delete/{id}")]
        public void DeleteStructure(int id) { _structureService.DeleteStructure(id); }

    }
}
