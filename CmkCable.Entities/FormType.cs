using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmkCable.Entities
{
    public class FormType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]  // FormType ismi için maksimum uzunluk
        public string FormTypes { get; set; }

        // ManagerMail ile ilişkiyi kuran MailFormType koleksiyonu
        public virtual ICollection<MailFormType> MailFormTypes { get; set; } = new List<MailFormType>();
    }
}
