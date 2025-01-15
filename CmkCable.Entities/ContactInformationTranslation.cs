using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class ContactInformationTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ContactInformation")]
        public int ContactInformationId { get; set; }
        [ForeignKey("Language")]
        public int LanguageId { get; set; }


        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100)]
        public string Department { get; set; }

        public virtual Language Language { get; set; }
        public virtual ContactInformation ContactInformation { get; set; }
    }
}
