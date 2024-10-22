using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities.CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCertificatesController : ControllerBase
    {
        private IProductCertificateService _productCertificateService;
        public ProductCertificatesController()
        {
            _productCertificateService = new ProductCertificateManager();
        }

        [HttpPost("create")]
        public ProductCertificate Create(ProductCertificate productCertificate) { return _productCertificateService.Create(productCertificate); }
    }
}
