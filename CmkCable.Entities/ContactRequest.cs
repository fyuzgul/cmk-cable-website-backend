using CmkCable.Entities.CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Entities
{
    public class ContactRequest
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
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

        [MaxLength(45)]  // IPv6 adresleri için yeterli uzunluk
        public string? IpAddress { get; set; }

        public bool Consent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        

    }
}
