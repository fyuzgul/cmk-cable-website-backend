using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class PdsDocumentDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string FileContent { get; set; }
        public List<PdsDocumentTranslationDTO> Translations { get; set; }
    }
}
