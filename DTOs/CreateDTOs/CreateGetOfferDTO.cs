using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs.CreateDTOs
{
    public class CreateGetOfferDTO
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string WorkEmail { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(150)]
        public string Company { get; set; }

        [Required]
        public int CompanyTypeId { get; set; }

        [Required]
        [MaxLength(20)]
        public string TelephoneNumber { get; set; }

        [Required]
        public int HelpTypeId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; }

        public string? IpAddress { get; set; }

        [Required]
        public bool AcikRiza { get; set; }
    }
}
