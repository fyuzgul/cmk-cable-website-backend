using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CmkCable.Entities
{
    public class CareerInformation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string MilitaryStatus { get; set; }
        public string DriverLicense { get; set; }
        public string TravelAvailability { get; set; }

        public string School { get; set; }
        public string Faculty { get; set; }
        public string GraduationDate { get; set; }
        public string Languages { get; set; }
        public string SoftwareSkills { get; set; }
        public string Seminars { get; set; }
        public string Department { get; set; }
        public string ReferenceSource { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Cv { get; set; }
        public string CvPath { get; set; }

        public bool Consent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual List<Experience> Experiences { get; set; } = new List<Experience>();
    }


    public class Experience
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Company { get; set; }
        public string Duration { get; set; }
        public string Position { get; set; }
        public int CareerInformationId { get; set; }
        public virtual CareerInformation CareerInformation { get; set; }
    }
}
