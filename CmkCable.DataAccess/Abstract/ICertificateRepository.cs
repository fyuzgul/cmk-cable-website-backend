using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface ICertificateRepository
    {
        List<Certificate> GetAllCertifacets();
        Certificate CreateCertificate(Certificate certificate);
        Certificate GetCertifacetById(int id);

        List<Certificate> GetAllCertificatesByTypeId(int typeId);

        Certificate UpdateCertificate(Certificate certificate);
        void DeleteCertificate(int id);

    }
}
