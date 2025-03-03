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
    public class ManagerMailsController : ControllerBase
    {
        public IManagerMailService managerMailService;
        public ManagerMailsController() => managerMailService = new ManagerMailManager();

        [HttpGet]
        public IActionResult GetAll() => Ok(managerMailService.GetAll());

        [HttpPost]
        public IActionResult Add([FromForm] ManagerMail managerMail, [FromForm] List<int> formTypeIds ) => Ok(managerMailService.Add(managerMail, formTypeIds));

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            managerMailService.Delete(id);
        }
        [HttpPut]
        public IActionResult Update([FromForm] ManagerMail managerMail, [FromForm] List<FormType> formTypes) => Ok(managerMailService.Update(managerMail, formTypes));
    }
}
