using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmkCable.Entities
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150)]
        public string Name { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public byte[] ImageData { get; set; }
    }
}
