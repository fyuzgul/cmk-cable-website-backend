using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace CmkCable.Entities
{
    public class TechnicalFeature
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "MaxTemp is required.")]
        public double MaxTemp { get; set; }

        [Required(ErrorMessage = "RatedVoltage is required.")]
        public double RatedVoltage { get; set; }

        [Required(ErrorMessage = "Section is required.")]
        public double Section { get; set; }

        [Required(ErrorMessage = "IsolationWallThickness is required.")]
        public double IsolationWallThickness { get; set; }

        [Required(ErrorMessage = "OuterSheathWallThickness is required.")]
        public double OuterSheathWallThickness { get; set; }

        [Required(ErrorMessage = "AverageExternalDiameter is required.")]
        public double AverageExternalDiameter { get; set; }

        [Required(ErrorMessage = "Resistance is required.")]
        public double Resistance { get; set; }


        [Required(ErrorMessage = "ApproximateWeight is required.")]
        public double ApproximateWeight { get; set; }
    }
}
