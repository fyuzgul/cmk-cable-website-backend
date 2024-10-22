using CmkCable.Entities.CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IProductCertificateService
    {
        ProductCertificate Create(ProductCertificate productCertificate);
    }
}
