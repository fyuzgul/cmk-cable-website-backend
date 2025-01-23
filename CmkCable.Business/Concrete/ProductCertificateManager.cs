using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities.CmkCable.Entities;


namespace CmkCable.Business.Concrete
{
    public class ProductCertificateManager : IProductCertificateService
    {
        private IProductCertificateRepository _productCertificateRepository;
        public ProductCertificateManager()
        {
            _productCertificateRepository = new ProductCertificateRepository();
        }

        public ProductCertificate Create(ProductCertificate productCertificate)
        {
           return _productCertificateRepository.Create(productCertificate);
        }

        public void Delete(int productId, int certificateId) { _productCertificateRepository.Delete(productId, certificateId); }
    }
}
