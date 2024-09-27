using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class CertificateTypeManager : ICertificateTypeService
    {
        private ICertificateTypeRepository _certificateTypeRepository;

        public CertificateTypeManager()
        {
            _certificateTypeRepository = new CertificateTypeRepository();
        }

        public CertificateType CreateCertificateType(CertificateType certificateType)
        {
            return _certificateTypeRepository.CreateCertificateType(certificateType);
        }

        public void DeleteCertificateType(int id)
        {
            _certificateTypeRepository?.DeleteCertificateType(id);
        }

        public List<CertificateType> GetAllCertificateTypes()
        {
            return _certificateTypeRepository.GetAllCertificateTypes();
        }

        public CertificateType GetCertificateTypeById(int id)
        {
           return _certificateTypeRepository.GetCertificateTypeById(id);
        }

        public CertificateType UpdateCertificateType(CertificateType certificateType)
        {
            return _certificateTypeRepository.UpdateCertificateType(certificateType);
        }
    }
}
