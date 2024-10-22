using CmkCable.Entities;
using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ProductDTO
    {
        public int Id {  get; set; }    
        public string Type { get; set; }
        public string Image { get; set; }   
        public string DetailImage { get; set; }
        public List<ProductTranslationDTO> UsageLocations { get; set; }   
        public CategoryDTO Category { get; set; }
        public List<Standart> Standarts { get; set; }
        public List<CertificateDTO> Certificates { get; set; } 
        public List<Structure> Structures { get; set; } 
    }
}
