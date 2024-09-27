using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        [StringLength(150)]
        public string Type { get; set; }

        [Required(ErrorMessage = "UsageLocations is required.")]
        [StringLength(150)]
        public string UsageLocations { get; set; }



        [NotMapped]
        public IFormFile Image { get; set; } 
        public byte[] ImageData { get; set; }

        [NotMapped]
        public IFormFile DetailImage { get; set; }
        public byte[] DetailImageData { get; set; }


        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }


        public ICollection<ProductStandart> ProductStandarts { get; set; }
        public ICollection<ProductCertificate> ProductCertificates { get; set; }
        public ICollection<ProductStructure> ProductStructures { get; set; }




    }
}
