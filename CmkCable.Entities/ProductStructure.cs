using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class ProductStructure
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }


        [ForeignKey(nameof(Structure))]
        public int StructureId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Structure Structure { get; set; }

    }
}
