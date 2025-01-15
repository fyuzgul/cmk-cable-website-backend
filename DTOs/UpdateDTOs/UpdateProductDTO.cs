using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DTOs.UpdateDTOs
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int CategoryId { get; set; }

        public IFormFile Image { get; set; }
        public IFormFile DetailImage { get; set; }

    }

}
