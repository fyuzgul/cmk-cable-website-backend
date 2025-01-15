using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface ICertificateTypeRepository
    {
        List<CertificateTypeDTO> GetAllCertificateTypes();
        CertificateType CreateCertificateType(CertificateType certificateType);

        CertificateType GetCertificateTypeById(int id);
        void DeleteCertificateType(int id);
        CertificateType UpdateCertificateType(CertificateType certificateType);
    }
}
