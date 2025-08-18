using System;

namespace DTOs
{
    public class GetOfferDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkEmail { get; set; }
        public int RoleId { get; set; }
        public string Country { get; set; }
        public string Company { get; set; }
        public int CompanyTypeId { get; set; }
        public string TelephoneNumber { get; set; }
        public int HelpTypeId { get; set; }
        public string Message { get; set; }
        public string? IpAddress { get; set; }
        public bool AcikRiza { get; set; }
        public DateTime CreatedAt { get; set; }

        public RoleDTO Role { get; set; }
        public CompanyTypeDTO CompanyType { get; set; }
        public HelpTypeDTO HelpType { get; set; }
    }
}
