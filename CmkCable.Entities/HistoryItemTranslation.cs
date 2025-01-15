using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class HistoryItemTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Language")]
        public int LanguageId { get; set; }
        [ForeignKey("HistoryItem")]
        public int HistoryItemId { get; set; }  

        public string Title { get; set; }
        public string Description { get; set; }

        public virtual HistoryItem HistoryItem { get; set; }
        public virtual Language Language { get; set; }  
    }
}
