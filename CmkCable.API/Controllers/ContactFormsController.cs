using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormsController : ControllerBase
    {
        private IContactRequestService _contactRequestService;
        public ContactFormsController()
        {
            _contactRequestService = new ContactRequestManager();
        }

        [HttpPost("create")]
        public IActionResult Post([FromBody] ContactRequest contactRequest)
        {
            _contactRequestService.CreateContactRequest(contactRequest);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAllContactRequests()
        {
            return Ok(_contactRequestService.GetAllContactRequests());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_contactRequestService.GetContactRequestById(id));
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _contactRequestService.DeleteContactRequest(id);
            return Ok();
        }
    }
}
