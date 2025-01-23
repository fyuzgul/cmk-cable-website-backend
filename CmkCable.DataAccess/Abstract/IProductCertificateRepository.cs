using CmkCable.Entities.CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IProductCertificateRepository
    {
        ProductCertificate Create(ProductCertificate productCertificate);
        void Delete(int productId, int certificateId);
    }
}
