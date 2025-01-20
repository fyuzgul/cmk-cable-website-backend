using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.UpdateDTOs
{
    public class UpdatePdsDocumentDTO
    {

        public int Id { get; set; }
        public IFormFile FileContent { get; set; }
        public IFormFile Image { get; set; }
    }
}
