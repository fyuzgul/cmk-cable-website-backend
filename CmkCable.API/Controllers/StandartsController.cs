using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StandartsController : ControllerBase
    {
        private readonly IStandartService _standartService;
        private readonly ILogger<StandartsController> _logger;

        public StandartsController(ILogger<StandartsController> logger)
        {
            _standartService = new StandartManager();
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Standart>> GetAllStandart()
        {
            _logger.LogInformation("Getting all standarts");
            return Ok(_standartService.GetAllStandart());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Standart> GetStandartById(int id)
        {
            _logger.LogInformation($"Getting standart by id: {id}");
            var standart = _standartService.GetStandartById(id);
            if (standart == null)
                return NotFound();
            return Ok(standart);
        }

        [HttpGet("byProduct/{id}")]
        [AllowAnonymous]
        public ActionResult<List<Standart>> GetStandartsByProducId(int id)
        {
            _logger.LogInformation($"Getting standarts by product id: {id}");
            return Ok(_standartService.GetStandartsByProducId(id));
        }

        [HttpGet("byCertificate/{id}")]
        [AllowAnonymous]
        public ActionResult<List<Standart>> GetStandartsByCertificateId(int id)
        {
            _logger.LogInformation($"Getting standarts by certificate id: {id}");
            return Ok(_standartService.GetStandartsByCertificateId(id));
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteStandart(int id)
        {
            _logger.LogInformation($"Deleting standart with id: {id}");
            _standartService.DeleteStandart(id);
            return Ok();
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public ActionResult<Standart> CreateStandart([FromBody] Standart standart)
        {
            _logger.LogInformation("Creating new standart");
            var createdStandart = _standartService.CreateStandart(standart);
            return CreatedAtAction(nameof(GetStandartById), new { id = createdStandart.Id }, createdStandart);
        }

        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public ActionResult<Standart> UpdateStandart([FromBody] Standart standart)
        {
            _logger.LogInformation($"Updating standart with id: {standart.Id}");
            var updatedStandart = _standartService.UpdateStandart(standart);
            if (updatedStandart == null)
                return NotFound();
            return Ok(updatedStandart);
        }
    }
}
