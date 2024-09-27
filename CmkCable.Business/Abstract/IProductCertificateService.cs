using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IProductCertificateService
    {
        List<ProductCertificate> GetProductCertificatesByProductId(int productId);
        ProductCertificate CreateProductCertificate(ProductCertificate ProductCertificate);

        List<ProductCertificate> GetCertificates();
        void DeleteProductCertificate(int id);

    }
}
