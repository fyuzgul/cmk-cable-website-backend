using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmkCable.Entities
{
    public class NavbarItemTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("NavbarItem")]
        public int NavbarItemId { get; set; }

        [ForeignKey("Language")]
        public int LanguageId { get; set; }

        public string Title { get; set; }

        public virtual NavbarItem NavbarItem { get; set; }
        public virtual Language Language { get; set; }
    }
}
