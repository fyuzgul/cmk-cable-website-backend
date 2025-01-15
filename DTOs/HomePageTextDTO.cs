using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class HomePageTextDTO
    {
        public int Id {  get; set; }    
        public string Name { get; set; }
        public List<HomePageTextTranslationDTO> Values {  get; set; }
    }
}
