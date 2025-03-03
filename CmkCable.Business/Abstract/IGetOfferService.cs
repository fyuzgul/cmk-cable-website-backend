using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Abstract
{
    public interface IGetOfferService
    {
        List<GetOffer> GetAllGetOffers();
        GetOffer GetOfferById(int id);
        GetOffer CreateGetOffer(GetOffer getOffer);
        void DeleteGetOffer(int id);

    }
}
