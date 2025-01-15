using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.CreateDTOs
{
    public class CreateCertificateTypeDTO
    {
        public string Name { get; set; }    
        public IFormFile Image {  get; set; }    
    }
}
