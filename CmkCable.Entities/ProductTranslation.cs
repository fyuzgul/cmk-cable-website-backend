using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmkCable.Entities
{
    public class ProductTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(150)]
        public string UsageLocations { get; set; }

        [ForeignKey(nameof(Language))]
        public int LanguageId { get; set; }

        public Language Language { get; set; } 

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; } 
    }
}
