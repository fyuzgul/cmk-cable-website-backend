using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCertificatesController : ControllerBase
    {
        private IProductCertificateService _productCertificateService;
        public ProductCertificatesController() { _productCertificateService = new ProductCertificateManager(); }

        [HttpGet]
        public List<ProductCertificate> GetCertificates() { return _productCertificateService.GetCertificates(); }  

        [HttpGet("{id}")]
        public List<ProductCertificate> Get(int id) { return _productCertificateService.GetProductCertificatesByProductId(id); }
        [HttpDelete("delete/{id}")]
        public void Delete(int id) {  _productCertificateService.DeleteProductCertificate(id); }


        [HttpPost("create")]
        public ProductCertificate CreateProductCertificate(ProductCertificate productCertificate) { return _productCertificateService.CreateProductCertificate(productCertificate); }

    }
}
