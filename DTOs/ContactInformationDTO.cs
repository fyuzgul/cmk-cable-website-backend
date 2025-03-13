using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class ContactInformationDTO
    {
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FaxNumber { get; set; }
        public string Department { get; set; }
        public List<ContactInformationTranslationDTO> Translations { get; set; }
    }

    public class ContactInformationDetailDTO
    {
        public int Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FaxNumber { get; set; }
        public List<TranslationDTO> Translations { get; set; }
    }

    public class TranslationDTO
    {
        public int LanguageId { get; set; }
        public string Department { get; set; }
    }
}
