using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface IGetOfferRepository
    {
        List<GetOffer> GetAllGetOffers();
        GetOffer GetOfferById(int id);
        GetOffer CreateGetOffer(GetOffer getOffer);
        void DeleteGetOffer(int id);
    }
}
