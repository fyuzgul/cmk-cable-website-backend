using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class CertificateManager : ICertificateService
    {
        private ICertificateRepository _certificateRepository;

        public CertificateManager()
        {
            _certificateRepository = new CertificateRepository();
        }

        public Certificate CreateCertificate(Certificate certificate)
        {
            return _certificateRepository.CreateCertificate(certificate);
        }

        public void DeleteCertificate(int id)
        {
            _certificateRepository.DeleteCertificate(id);
        }

        public List<Certificate> GetAllCertifacets()
        {
            return _certificateRepository.GetAllCertifacets(); 
        }

        public List<Certificate> GetAllCertificatesByTypeId(int id)
        {
          return _certificateRepository.GetAllCertificatesByTypeId(id);
        }

        public Certificate GetCertifacetById(int id)
        {
            return _certificateRepository.GetCertifacetById((int)id);
        }

        public Certificate UpdateCertificate(Certificate certificate)
        {
            return _certificateRepository.UpdateCertificate(certificate);
        }
    }
}
