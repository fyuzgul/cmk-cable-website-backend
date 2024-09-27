using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class ProductCertificateManager : IProductCertificateService
    {
        private IProductCertificateRepository _productCertificateRepository;
        public ProductCertificateManager() { _productCertificateRepository = new ProductCertificateRepository(); }

        public ProductCertificate CreateProductCertificate(ProductCertificate ProductCertificate)
        {
           return _productCertificateRepository.CreateProductCertificate(ProductCertificate);
        }

        public void DeleteProductCertificate(int id)
        {
            _productCertificateRepository.DeleteProductCertificate(id);
        }

        public List<ProductCertificate> GetProductCertificatesByProductId(int productId)
        {
            return _productCertificateRepository.GetProductCertificatesByProductId(productId);
        }

        public List<ProductCertificate> GetCertificates()
        {
            return  _productCertificateRepository.GetCertificates();
        }
    }
}
