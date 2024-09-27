using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class CertificateRepository : ICertificateRepository
    {
        public Certificate CreateCertificate(Certificate certificate)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Certificates.Add(certificate);
                cmkCableDbContext.SaveChanges();
                return certificate;
            }
        }

        public void DeleteCertificate(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedCertificates = cmkCableDbContext.Certificates.Find(id);
                cmkCableDbContext.Certificates.Remove(deletedCertificates);
                cmkCableDbContext.SaveChanges() ;
            }

        }

        public List<Certificate> GetAllCertifacets()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Certificates.ToList();
            }
        }

        public List<Certificate> GetAllCertificatesByTypeId(int typeId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Certificates
                    .Where(c => c.TypeId == typeId) 
                    .ToList(); 
            }
        }



        public Certificate GetCertifacetById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Certificates.Find(id);
            }
        }

        public Certificate UpdateCertificate(Certificate certificate)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Certificates.Update(certificate);
                cmkCableDbContext.SaveChanges();
                return certificate;
            }
        }
    }
}
