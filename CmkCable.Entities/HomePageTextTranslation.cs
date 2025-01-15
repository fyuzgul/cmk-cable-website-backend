using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class HomePageTextTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Value { get; set; }

        [ForeignKey("HomePageText")]
        public int TextId { get; set; }

        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public virtual HomePageText HomePageText { get; set; }

    }
}
