using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs.CreateDTOs
{
    public class CreateHomePageTextWithTranslationsDTO
    {
        public string Name { get; set; }
        public List<HomePageTextTranslationDTO> Translations { get; set; }
    }
}
