using Microsoft.AspNetCore.Http;

namespace DTOs.CreateDTOs
{
    public class CreateProductDto
    {
        public string Type { get; set; }

        public IFormFile Image { get; set; }

        public IFormFile DetailImage { get; set; }

        public int CategoryId { get; set; }
    }
}
