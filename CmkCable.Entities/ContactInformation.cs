using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class ContactInformation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string? PhoneNumber { get; set; }

        public string Email { get; set; }
        public string FaxNumber { get; set; }

        public virtual ICollection<ContactInformationTranslation> Translations { get; set; } = new List<ContactInformationTranslation>();
    }
}
