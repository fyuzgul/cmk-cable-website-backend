using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetOffersController : ControllerBase
    {
        private IGetOfferService _getOfferService;
        public GetOffersController()
        {
            _getOfferService = new GetOfferManager();
        }

        [HttpGet]
        public List<GetOffer> GetAllGetOffers()
        {
            return _getOfferService.GetAllGetOffers();
        }

        [HttpGet("{id}")]
        public GetOffer Get(int id)
        {
            return _getOfferService.GetOfferById(id);
        }
        [HttpPost("create")]
        public GetOffer Post([FromBody] GetOffer getOffer)
        {
            return _getOfferService.CreateGetOffer(getOffer);
        }
        [HttpDelete("delete/{id}")]
        public void Delete(int id)
        {
            _getOfferService.DeleteGetOffer(id);
        }
    }
}
