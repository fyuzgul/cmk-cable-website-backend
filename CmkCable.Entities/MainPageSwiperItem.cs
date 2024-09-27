using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CmkCable.Entities
{
    public class MainPageSwiperItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(150)]
        public string Description { get; set; } 

        [NotMapped]
        public IFormFile Video { get; set; }
        public byte[] VideoData { get; set; }
    }
}
