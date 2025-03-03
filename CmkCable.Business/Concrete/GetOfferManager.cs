using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System.Collections.Generic;

namespace CmkCable.Business.Concrete
{
    public class GetOfferManager : IGetOfferService
    {
        private IGetOfferRepository _getOfferRepository;
        public GetOfferManager()
        {
            _getOfferRepository = new GetOfferRepository();
        }
        public GetOffer CreateGetOffer(GetOffer getOffer)
        {
            return _getOfferRepository.CreateGetOffer(getOffer);
        }

        public void DeleteGetOffer(int id)
        {
            
            _getOfferRepository.DeleteGetOffer(id);
        }

        public List<GetOffer> GetAllGetOffers()
        {
            return _getOfferRepository.GetAllGetOffers();
        }

        public GetOffer GetOfferById(int id)
        {
            return _getOfferRepository.GetOfferById(id);    
        }
    }
}
