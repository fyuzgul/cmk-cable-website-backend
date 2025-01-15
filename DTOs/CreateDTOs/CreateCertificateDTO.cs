using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTOs.CreateDTOs
{
    public class CreateCertificateDTO
    {
        public string Name { get; set; }
        public int TypeId { get; set; }
        public IFormFile FileContent { get; set; }
        public IFormFile Image { get; set; }

    }
}
