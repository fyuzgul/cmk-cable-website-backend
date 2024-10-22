using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class AboutUsItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  
        public int Id { get; set; } 
        public string Title { get; set; }    
        public string Description { get; set; }
        public string SubDescription { get; set; }  

    }
}
