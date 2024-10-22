using CmkCable.DataAccess.Abstract;
using CmkCable.Entities.CmkCable.Entities;
using System;
using System.Collections.Generic;
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
    }
}
