using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CmkCable.Entities
{
    public class NavbarItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Route { get; set; }

        public virtual ICollection<NavbarItemTranslation> Translations { get; set; }
    }

}
