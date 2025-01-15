using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface ICertificateTypeService
    {
        List<CertificateTypeDTO> GetAllCertificateTypes();
        CertificateType CreateCertificateType(CertificateType certificateType);
        CertificateType GetCertificateTypeById(int id);
        void DeleteCertificateType(int id);
        CertificateType UpdateCertificateType(CertificateType certificateType);
    }
}
