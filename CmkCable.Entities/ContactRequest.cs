using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class ContactRequest
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        [MaxLength(45)]  // IPv6 adresleri için yeterli uzunluk
        public string? IpAddress { get; set; }

        public bool Consent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
