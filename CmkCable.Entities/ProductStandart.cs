using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmkCable.Entities
{
    public class ProductStandart
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }


        [ForeignKey(nameof(Standart))]
        public int StandartId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Standart Standart { get; set; }
    }
}
