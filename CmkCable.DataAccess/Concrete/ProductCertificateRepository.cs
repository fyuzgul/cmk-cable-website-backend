using CmkCable.DataAccess.Abstract;
using CmkCable.Entities.CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ProductCertificateRepository : IProductCertificateRepository
    {
        public ProductCertificate Create(ProductCertificate productCertificate)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Add(productCertificate);
                cmkCableDbContext.SaveChanges();
                return productCertificate;  
            }

        }

        public void Delete(int productId, int certificateId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var productCertificate = cmkCableDbContext.ProductCertificates
                    .FirstOrDefault(pc => pc.ProductId == productId && pc.CertificateId == certificateId);

                if (productCertificate != null)
                {
                    cmkCableDbContext.ProductCertificates.Remove(productCertificate);
                    cmkCableDbContext.SaveChanges();
                }
            }
        }

    }
}
