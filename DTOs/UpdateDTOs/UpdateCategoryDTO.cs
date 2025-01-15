using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.UpdateDTOs
{
    public class UpdateCategoryDTO
    {
        public int Id {  get; set; }
        public IFormFile Image { get; set; }
    }
}
