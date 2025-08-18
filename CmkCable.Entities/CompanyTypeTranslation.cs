using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class CompanyTypeTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int CompanyTypeId { get; set; }
        
        [Required]
        public int LanguageId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        // Navigation properties
        [ForeignKey("CompanyTypeId")]
        public virtual CompanyType CompanyType { get; set; }
        
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }
}
