using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CmkCable.Entities
{
    public class CareerInformation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
        [MaxLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [MaxLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        public string TelephoneNumber { get; set; }
        
        [Required(ErrorMessage = "Email adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [MaxLength(100, ErrorMessage = "Email adresi en fazla 100 karakter olabilir")]
        public string Email { get; set; }
        
        [MaxLength(50, ErrorMessage = "Cinsiyet en fazla 50 karakter olabilir")]
        public string Gender { get; set; }
        
        [MaxLength(50, ErrorMessage = "Medeni durum en fazla 50 karakter olabilir")]
        public string MaritalStatus { get; set; }
        
        [MaxLength(50, ErrorMessage = "Askerlik durumu en fazla 50 karakter olabilir")]
        public string MilitaryStatus { get; set; }
        
        [MaxLength(50, ErrorMessage = "Sürücü belgesi en fazla 50 karakter olabilir")]
        public string DriverLicense { get; set; }
        
        [MaxLength(50, ErrorMessage = "Seyahat durumu en fazla 50 karakter olabilir")]
        public string TravelAvailability { get; set; }

        [MaxLength(100, ErrorMessage = "Departman en fazla 100 karakter olabilir")]
        public string Department { get; set; }
        
        [MaxLength(200, ErrorMessage = "Referans kaynağı en fazla 200 karakter olabilir")]
        public string ReferenceSource { get; set; }
        
        [MaxLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Cv { get; set; }
        
        [MaxLength(500, ErrorMessage = "CV yolu en fazla 500 karakter olabilir")]
        public string CvPath { get; set; }

        [MaxLength(45)]  // IPv6 adresleri için yeterli uzunluk
        public string? IpAddress { get; set; }

        [Required(ErrorMessage = "Açık rıza zorunludur")]
        public bool Consent { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
