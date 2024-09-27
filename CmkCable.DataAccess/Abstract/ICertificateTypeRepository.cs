using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface ICertificateTypeRepository
    {
        List<CertificateType> GetAllCertificateTypes();
        CertificateType CreateCertificateType(CertificateType certificateType);

        CertificateType GetCertificateTypeById(int id);
        void DeleteCertificateType(int id);
        CertificateType UpdateCertificateType(CertificateType certificateType);
    }
}
