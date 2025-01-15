using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.CreateDTOs
{
    public class CreatePdsDocumentDTO
    {
        public IFormFile FileContent { get; set; }
        public IFormFile Image { get; set; }
    }
}
