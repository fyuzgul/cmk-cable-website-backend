using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class CareerForm
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Marital Status is required.")]
        public string MaritalStatus { get; set; }

        [Required(ErrorMessage = "Military Status is required.")]
        public string MilitaryStatus { get; set; }

        [Required(ErrorMessage = "Driver License is required.")]
        public string DriverLicense { get; set; }

        [Required(ErrorMessage = "Travel Availability is required.")]
        public string TravelAvailability { get; set; }

        public string University { get; set; }
        public string Faculty { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Languages { get; set; }
        public string SoftwareSkills { get; set; }
        public string Seminars { get; set; }

        public ICollection<Experience> Experiences { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Reference Source is required.")]
        public string ReferenceSource { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [NotMapped]
        public IFormFile CV { get; set; }

        public byte[] CVData { get; set; }

        [Required(ErrorMessage = "Consent is required.")]
        public bool Consent { get; set; }
    }

    public class Experience
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Company { get; set; }
        public string Duration { get; set; }
        public string Position { get; set; }

        public int CareerApplicationId { get; set; }
        [ForeignKey("CareerApplicationId")]
        public CareerForm CareerForm { get; set; }
    }
}
