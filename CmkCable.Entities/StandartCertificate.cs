using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class StandartCertificate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {  get; set; }
        [ForeignKey(nameof(Certificate))]
        public int CertificateId { get; set; }
        public Certificate Certificate { get; set; }

        [ForeignKey(nameof(Standart))]
        public int StandartId { get; set; }
        public Standart Standart { get; set; }
    }
}
