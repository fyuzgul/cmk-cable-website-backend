using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmkCable.Entities
{
    public class NavbarItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Route { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual NavbarItem ParentItem { get; set; }

        public virtual ICollection<NavbarItem> SubItems { get; set; }

        public virtual ICollection<NavbarItemTranslation> Translations { get; set; }
    }
}
