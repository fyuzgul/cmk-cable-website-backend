using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class Standart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [ForeignKey(nameof(Product))]    
        public int ProductId { get; set; }

        [ForeignKey(nameof(Certificate))]
        public int CertificateId { get; set; }  

        public virtual Product Product { get; set; }
        public virtual Certificate Certificate { get; set; }

    }
}
