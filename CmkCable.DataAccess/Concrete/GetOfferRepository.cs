using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class GetOfferRepository : IGetOfferRepository
    {
        public GetOffer CreateGetOffer(GetOffer getOffer)
        {
            using (var context = new CmkCableDbContext())
            {
                context.GetOffers.Add(getOffer);
                context.SaveChanges();
                return getOffer;
            }
        }

        public void DeleteGetOffer(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                context.GetOffers.Remove(context.GetOffers.Find(id));
                context.SaveChanges();
            }
        }

        public List<GetOffer> GetAllGetOffers()
        {
            using(var context = new CmkCableDbContext())
            {
                return context.GetOffers.ToList();
            }
        }
        public GetOffer GetOfferById(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                return context.GetOffers.Find(id);
            }
        }
    }
}
