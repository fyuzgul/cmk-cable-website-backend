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

        public string Department { get; set; }
        public string ReferenceSource { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Cv { get; set; }
        public string CvPath { get; set; }

        [MaxLength(45)]  // IPv6 adresleri için yeterli uzunluk
        public string? IpAddress { get; set; }

        public bool Consent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
