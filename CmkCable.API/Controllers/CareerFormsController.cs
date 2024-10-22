using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CareerFormsController : ControllerBase
    {
        private ICareerFormService _careerFormService;
        public CareerFormsController()
        {
            _careerFormService = new CareerFormManager();
        }

        [HttpGet("{id}")]
        public CareerForm GetCareerFormById(int id) { return _careerFormService.GetCareerFormById(id); }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCareerForm([FromForm] CareerForm careerForm)
        {
            if (careerForm.CV == null || careerForm.CV.Length == 0)
                return BadRequest("No CV uploaded.");
            byte[] cvBytes;

            using (var memoryStream = new MemoryStream())
            {
                await careerForm.CV.CopyToAsync(memoryStream);
                cvBytes = memoryStream.ToArray();
            }
            var newCareerForm = new CareerForm
            {
                FullName = careerForm.FullName,
                TelephoneNumber = careerForm.TelephoneNumber,
                Email = careerForm.Email,
                Gender = careerForm.Gender,
                MaritalStatus = careerForm.MaritalStatus,
                MilitaryStatus = careerForm.MilitaryStatus,
                DriverLicense = careerForm.DriverLicense,
                TravelAvailability = careerForm.TravelAvailability,
                University = careerForm.University,
                Faculty = careerForm.Faculty,
                GraduationDate = careerForm.GraduationDate,
                Languages = careerForm.Languages,
                SoftwareSkills = careerForm.SoftwareSkills,
                Seminars = careerForm.Seminars,
                Department = careerForm.Department,
                ReferenceSource = careerForm.ReferenceSource,
                Description = careerForm.Description,
                CVData = cvBytes,  
                Consent = careerForm.Consent,
                Experiences = careerForm.Experiences
            };
            var createdCareerForm = _careerFormService.CreateCareerForm(newCareerForm);
            return Ok(createdCareerForm);
        }
    }
}
