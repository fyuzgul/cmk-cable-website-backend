using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface ICertificateService
    {
        List<CertificateDTO> GetAllCertifacets();
        Certificate CreateCertificate(Certificate certificate);
        Certificate GetCertifacetById(int id);
        List<Certificate> GetAllCertificatesByTypeId(int id);

        Certificate UpdateCertificate(Certificate certificate);
        void DeleteCertificate(int id);
    }
}
