using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductCertificateRepository : IProductCertificateRepository
    {

        public ProductCertificate CreateProductCertificate(ProductCertificate productCertificate)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.ProductCertificates.Add(productCertificate);
                cmkCableDbContext.SaveChanges();
                return productCertificate;
            }
        }

        public void DeleteProductCertificate(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedRecord = cmkCableDbContext.ProductCertificates.Find(id);
                cmkCableDbContext.ProductCertificates.Remove(deletedRecord);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<ProductCertificate> GetCertificates()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.ProductCertificates.ToList();
            }
        }

        public List<ProductCertificate> GetProductCertificatesByProductId(int productId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.ProductCertificates
                    .Where(pc => pc.ProductId == productId)
                    .ToList();
            }
        }
    }
}

