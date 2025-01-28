using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
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

        public List<CertificateDTO> GetAllCertifacets()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                // Sertifikaları al
                var certificates = cmkCableDbContext.Certificates.ToList();
                var certificateDTOs = new List<CertificateDTO>();

                foreach (var certificate in certificates)
                {
                    var productIds = cmkCableDbContext.ProductCertificates
                                                       .Where(pc => pc.CertificateId == certificate.Id)
                                                       .Select(pc => pc.ProductId)
                                                       .ToList();

                    var productNames = cmkCableDbContext.Products
                                                        .Where(p => productIds.Contains(p.Id))
                                                        .Select(p => p.Type)
                                                        .ToList();

                    var certificateDTO = new CertificateDTO
                    {
                        Id = certificate.Id,
                        Name = certificate.Name,
                        FileContent = certificate.FileContent,
                        Image = certificate.Image,
                        CertificateType = new CertificateTypeDTO
                        {
                            Id = certificate.TypeId,
                            Name = cmkCableDbContext.CertificateTypes
                                        .Where(ct => ct.Id == certificate.TypeId)
                                        .Select(ct => ct.Name)
                                        .FirstOrDefault()
                        },
                        ProductNames = productNames
                    };

                    certificateDTOs.Add(certificateDTO);
                }

                return certificateDTOs;
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
                var existingCertificate = cmkCableDbContext.Certificates.Find(certificate.Id);
                if (existingCertificate == null)
                {
                    throw new Exception($"Certificate with ID {certificate.Id} not found");
                }

                // Update the properties
                existingCertificate.Name = certificate.Name;
                existingCertificate.FileContent = certificate.FileContent;
                existingCertificate.Image = certificate.Image;
                existingCertificate.TypeId = certificate.TypeId;

                cmkCableDbContext.SaveChanges();

                return existingCertificate;
            }
        }

    }
}
