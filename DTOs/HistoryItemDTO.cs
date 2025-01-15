using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class HistoryItemDTO
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Image { get; set; }
        public List<HistoryItemTranslationDTO> Translations { get; set; }
    }
}
