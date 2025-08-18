using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ContactRequestDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string IpAddress { get; set; }
        public bool Consent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

