using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class Structure
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        public virtual Product Product { get; set; }  
        public virtual Language Language { get; set; }
    }
}
