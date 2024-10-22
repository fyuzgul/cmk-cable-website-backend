using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class CertificateStandart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey(nameof(Certificate))]
        public int CertificateId { get; set; }

        [ForeignKey(nameof(Standart))]
        public int StandartId { get; set; }
   
        public virtual Certificate Certificate { get; set; }
        public virtual Standart Standart { get; set; }  
    }
}
