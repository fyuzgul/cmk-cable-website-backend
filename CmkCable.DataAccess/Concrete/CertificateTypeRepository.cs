using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class CertificateTypeRepository : ICertificateTypeRepository
    {
        public CertificateType CreateCertificateType(CertificateType certificateType)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.CertificateTypes.Add(certificateType);
                cmkCableDbContext.SaveChanges();
                return certificateType;
            }
        }

        public void DeleteCertificateType(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedCertificateType = cmkCableDbContext.CertificateTypes.Find(id);
                cmkCableDbContext.CertificateTypes.Remove(deletedCertificateType);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<CertificateType> GetAllCertificateTypes()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.CertificateTypes.ToList();
            }
        }

        public CertificateType GetCertificateTypeById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.CertificateTypes.Find(id);
            }
        }

        public CertificateType UpdateCertificateType(CertificateType certificateType)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.CertificateTypes.Update(certificateType);
                cmkCableDbContext.SaveChanges();
                return certificateType;
            }
        }
    }
}
