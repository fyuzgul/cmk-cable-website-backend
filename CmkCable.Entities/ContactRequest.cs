using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Entities
{
    public class ContactRequest
    {
        public string FullName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
