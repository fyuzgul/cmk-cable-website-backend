using CmkCable.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class TechnicalFeature
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "MaxTemp is required.")]
    public string MaxTemp { get; set; }

    [Required(ErrorMessage = "RatedVoltage is required.")]
    public string RatedVoltage { get; set; }

    [Required(ErrorMessage = "Section is required.")]
    public string Section { get; set; }

    [Required(ErrorMessage = "IsolationWallThickness is required.")]
    public string IsolationWallThickness { get; set; }

    [Required(ErrorMessage = "OuterSheathWallThickness is required.")]
    public string OuterSheathWallThickness { get; set; }

    [Required(ErrorMessage = "AverageExternalDiameter is required.")]
    public string AverageExternalDiameter { get; set; }

    [Required(ErrorMessage = "Resistance is required.")]
    public string Resistance { get; set; }

    [Required(ErrorMessage = "ApproximateWeight is required.")]
    public string ApproximateWeight { get; set; }
    [Required(ErrorMessage = "Current Carrying Cap. (A) is required")]
    public string CurrentCarryingCap { get; set; }

}
