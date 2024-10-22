using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CmkCable.Entities
{
    public class HistoryItem
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Year { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public byte[] ImageData { get; set; }
    }
}
