using CmkCable.Entities;
using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public List<CategoryTranslationDTO> Translations { get; set; } = new List<CategoryTranslationDTO>();    
    }   
}
    