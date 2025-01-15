using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class StructureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public List<StructureTranslationDTO> Structures { get; set; }
        
    }
}
