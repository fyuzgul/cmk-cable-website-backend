using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class PdsDocumentTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(150)]
        public string Name { get; set; }

        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        [ForeignKey("PdsDocument")]
        public int PdsId { get; set; }
        public virtual Language Language { get; set; }
        public virtual PdsDocument PdsDocument { get; set; }
    }
}
