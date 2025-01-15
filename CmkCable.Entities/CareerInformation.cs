using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CmkCable.Entities
{
    public class CareerInformation
    {
        public string FullName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string MilitaryStatus { get; set; }
        public string DriverLicense { get; set; }
        public string TravelAvailability { get; set; }

        public Education Education { get; set; }
        public List<Experience> Experiences { get; set; }

        public string Department { get; set; }
        public string ReferenceSource { get; set; }
        public string Description { get; set; }

        public IFormFile Cv { get; set; }
        public bool Consent { get; set; }
    }

    public class Education
    {
        public string School { get; set; }
        public string Faculty { get; set; }
        public string GraduationDate { get; set; }
        public string Languages { get; set; }
        public string SoftwareSkills { get; set; }
        public string Seminars { get; set; }
    }

    public class Experience
    {
        public string Company { get; set; }
        public string Duration { get; set; }
        public string Position { get; set; }
    }
}
