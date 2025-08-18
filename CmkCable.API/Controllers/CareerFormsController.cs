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
    public class CareerFormsController : ControllerBase
    {
        private readonly ICareerInformationService _careerInformationService;
        public CareerFormsController()
        {
            _careerInformationService = new CareerInformationManager(); 
        }

        [HttpGet]
        public IActionResult Get()
        {
            var data = _careerInformationService.GetAllCareerInformation();
            return Ok(data);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id) {_careerInformationService.GetCareerInformationById(id); return Ok(); }

        [HttpPost]
        public IActionResult Post([FromForm] CareerInformation careerInformation) { _careerInformationService.CreateCareerInformation(careerInformation); return Ok(); }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id) { _careerInformationService.DeleteCareerInformation(id); return Ok(); }
    }
}
