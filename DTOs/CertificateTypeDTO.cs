using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class CertificateTypeDTO
    {
        public int Id {  get; set; }    
        public string Name { get; set; }
        public string Image{ get; set; }
        public List<CertificateDTO> Certificates { get; set; }  
    }
}
