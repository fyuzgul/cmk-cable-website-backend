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
    public class TechnicalFeaturesController : ControllerBase
    {
        private ITechnicalFeatureService _technicalFeatureService;
        public TechnicalFeaturesController() { _technicalFeatureService = new TechnicalFeatureManager(); }
        [HttpGet("byProduct/{id}")]
        public List<TechnicalFeature> GetAllFeaturesByProductId(int id) {
            return _technicalFeatureService.GetAllFeaturesByProductId(id); 
        }
        [HttpGet]
        public List<TechnicalFeature> GetAllFeatures() { return _technicalFeatureService.GetAllFeatures();}
        [HttpPost("create")]
        public TechnicalFeature CreateTechnicalFeature(TechnicalFeature technicalFeature) { return _technicalFeatureService.CreateTechnicalFeature(technicalFeature); }
        [HttpPut("update")]
        public TechnicalFeature UpdateTechnicalFeature(TechnicalFeature technicalFeature) { return _technicalFeatureService.UpdateTechnicalFeature(technicalFeature); }
        [HttpDelete("delete/{id}")]
        public void DeleteDeleteTechnicalFeature(int id) { _technicalFeatureService.DeleteTechnicalFeature(id); }

    }
}
