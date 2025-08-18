using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.CreateDTOs
{
    public class CreateContactRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Street { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Postcode { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string TelephoneNumber { get; set; }
        
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
        
        public string IpAddress { get; set; }
        
        [Required]
        public bool Consent { get; set; }
    }
}

