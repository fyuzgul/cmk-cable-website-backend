using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmkCable.Entities
{
    public class Certificate
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150)]
        public string Name { get; set; }

        [ForeignKey(nameof(CertificateType))]
        public int TypeId {  get; set; }

        [NotMapped]
        public IFormFile FileContent { get; set; }

        public byte[] FileContentData { get; set; }


        [NotMapped]
        public IFormFile Image { get; set; } 

        public byte[] ImageData { get; set; }

    }
}
