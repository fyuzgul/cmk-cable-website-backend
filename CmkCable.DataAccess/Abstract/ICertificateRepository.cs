using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface ICertificateRepository
    {
        List<CertificateDTO> GetAllCertifacets();
        Certificate CreateCertificate(Certificate certificate);
        Certificate GetCertifacetById(int id);

        List<Certificate> GetAllCertificatesByTypeId(int typeId);

        Certificate UpdateCertificate(Certificate certificate);
        void DeleteCertificate(int id);

    }
}
