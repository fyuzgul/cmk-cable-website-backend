using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class Language
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public int Id { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
    }
}
