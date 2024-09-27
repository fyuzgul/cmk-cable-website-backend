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
    public class StandartsController : ControllerBase
    {
        private IStandartService _standartService;
        public StandartsController() { _standartService = new StandartManager(); }

        [HttpGet]
        public List<Standart> GetAllStandart() {  return _standartService.GetAllStandart();}

        [HttpGet("{id}")]
        public Standart GetStandartById(int id) { return _standartService.GetStandartById(id); }

        [HttpGet("byProduct/{id}")]
        public List<Standart> GetStandartsByProducId(int id) { return _standartService.GetStandartsByProducId(id); }
        [HttpGet("byCertificate/{id}")]
        public List<Standart> GetStandartsByCertificateId(int id) { return _standartService.GetStandartsByCertificateId(id); }
        [HttpDelete("delete/{id}")]
        public void DeleteStandart(int id) { _standartService.DeleteStandart(id); }
        [HttpPost("create")]
        public Standart CreateStandart(Standart standart) { return _standartService.CreateStandart(standart); }
        [HttpPut("update")]
        public Standart UpdateStandart(Standart standart) { return _standartService.UpdateStandart(standart); }

    }
}
