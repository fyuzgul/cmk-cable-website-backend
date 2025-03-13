using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class ContactInformationCreateDTO
    {
        [StringLength(100)]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        public string FaxNumber { get; set; }

        [Required(ErrorMessage = "En az bir dil için çeviri girilmelidir.")]
        [MinLength(1, ErrorMessage = "En az bir dil için çeviri girilmelidir.")]
        public List<ContactInformationTranslationDTO> Translations { get; set; }
    }

    public class ContactInformationTranslationDTO
    {
        [Required(ErrorMessage = "Dil ID zorunludur.")]
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Departman adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Departman adı en fazla 100 karakter olabilir.")]
        public string Department { get; set; }
    }
} 