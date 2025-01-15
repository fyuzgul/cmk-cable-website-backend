using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

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

        public List<CertificateTypeDTO> GetAllCertificateTypes()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var certificateTypes = cmkCableDbContext.CertificateTypes.ToList();

                var certificateTypeDTOs = certificateTypes.Select(ct => new CertificateTypeDTO
                {
                    Id = ct.Id,
                    Name = ct.Name,
                    Image = ct.Image,
                    Certificates = cmkCableDbContext.Certificates
                            .Where(c => c.TypeId == ct.Id)
                            .Select(c => new CertificateDTO
                            {
                                Id = c.Id,
                                Image = c.Image,
                                FileContent = c.FileContent,
                                Name = c.Name,
                                ProductNames = cmkCableDbContext.ProductCertificates
                                    .Where(pc => pc.CertificateId == c.Id) // Belirli sertifikaya ait ürünleri bul
                                    .Select(pc => pc.ProductId) // Ürün ID'lerini al
                                    .Join(cmkCableDbContext.Products,
                                          productId => productId,
                                          product => product.Id,
                                          (productId, product) => product.Type) // Ürün isimlerini al
                                    .ToList() // List haline getir
                            }).ToList()
                                    }).ToList();


                return certificateTypeDTOs;
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
