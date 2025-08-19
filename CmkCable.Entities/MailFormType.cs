using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmkCable.Entities
{
    public class MailFormType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // ForeignKey ilişkileri
        [ForeignKey("ManagerMail")]
        public int MailId { get; set; }
        public ManagerMail ManagerMail { get; set; }

        [ForeignKey("FormType")]
        public int FormTypeId { get; set; }
        public FormType FormType { get; set; }
    }
}
