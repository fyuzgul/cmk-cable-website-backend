using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class RoleTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public int RoleId { get; set; }
        
        [Required]
        public int LanguageId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        // Navigation properties
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }
}
