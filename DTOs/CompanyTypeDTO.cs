using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class CompanyTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


