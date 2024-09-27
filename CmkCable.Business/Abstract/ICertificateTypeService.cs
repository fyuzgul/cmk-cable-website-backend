using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface ICertificateTypeService
    {
        List<CertificateType> GetAllCertificateTypes();
        CertificateType CreateCertificateType(CertificateType certificateType);
        CertificateType GetCertificateTypeById(int id);
        void DeleteCertificateType(int id);
        CertificateType UpdateCertificateType(CertificateType certificateType);
    }
}
