using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class Structure
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(150)]
        public string Description { get; set; }
        public ICollection<ProductStructure> ProductStructures { get; set; }
    }
}
