using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class CertificateDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }    
        public CertificateTypeDTO CertificateType { get; set; }
        public byte[] FileContent {  get; set; }    
        public byte[] Image {  get; set; }  
        public List<string> ProductNames { get; set; }
    }
}
