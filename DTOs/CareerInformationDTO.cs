using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class CareerInformationDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string MilitaryStatus { get; set; }
        public string DriverLicense { get; set; }
        public string TravelAvailability { get; set; }
        public List<Experience> Experiences { get; set; }
        public string School { get; set; }
        public string Faculty { get; set; }
        public string GraduationDate { get; set; }
        public string Languages { get; set; }
        public string SoftwareSkills { get; set; }
        public string Seminars { get; set; }
        public string Department { get; set; }
        public string ReferenceSource { get; set; }
        public string Description { get; set; }
        public string CvPath { get; set; }

        public bool Consent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
