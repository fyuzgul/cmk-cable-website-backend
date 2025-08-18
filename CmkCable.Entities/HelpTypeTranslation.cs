using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class HelpTypeTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int HelpTypeId { get; set; }
        
        [Required]
        public int LanguageId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        // Navigation properties
        [ForeignKey("HelpTypeId")]
        public virtual HelpType HelpType { get; set; }
        
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }
}
