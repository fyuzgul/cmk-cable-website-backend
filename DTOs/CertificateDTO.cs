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
        public string FileContent {  get; set; }    
        public string Image {  get; set; }  
        public List<string> ProductNames { get; set; }
    }
}
