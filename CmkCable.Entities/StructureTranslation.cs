using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class StructureTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        [ForeignKey("Structure")]
        public int StructureId { get; set; }
        public string Description { get; set; }
    }
}
