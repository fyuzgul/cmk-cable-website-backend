using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.UpdateDTOs
{
    public class UpdateCertificateDTO
    {
        public int Id { get; set; } 
        public string Name { get; set; }    
        public int TypeId { get; set; } 
        public IFormFile Image { get; set; }
        public IFormFile FileContent { get; set; }
    }
}
