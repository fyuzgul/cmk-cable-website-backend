using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IProductCertificateRepository
    {
        List<ProductCertificate> GetCertificates(); 
        List<ProductCertificate> GetProductCertificatesByProductId(int productId);

        ProductCertificate CreateProductCertificate(ProductCertificate productCertificate);
        void DeleteProductCertificate(int id);
    }
}
