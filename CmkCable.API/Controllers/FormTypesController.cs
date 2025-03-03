using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormTypesController : ControllerBase
    {
        private IFormTypesService _formTypesService;
        public FormTypesController()
        {
            _formTypesService = new FormTypesManager();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var formTypes = _formTypesService.GetAll();
            return Ok(formTypes);
        }
    }
}
