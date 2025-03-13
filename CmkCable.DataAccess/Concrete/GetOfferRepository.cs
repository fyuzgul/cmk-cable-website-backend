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
                return context.GetOffers
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new GetOffer
                    {
                        Id = o.Id,
                        AdSoyad = o.AdSoyad,
                        FirmaAdi = o.FirmaAdi,
                        Telefon = o.Telefon,
                        Email = o.Email,
                        Unvan = o.Unvan,
                        Ulke = o.Ulke,
                        Kablolar = o.Kablolar,
                        Aciklama = o.Aciklama,
                        Lme = o.Lme,
                        ParaBirimleri = o.ParaBirimleri,
                        TeslimSekli = o.TeslimSekli,
                        TeslimYeri = o.TeslimYeri,
                        OdemeSekli = o.OdemeSekli,
                        Ambalajlama = o.Ambalajlama,
                        AcikRiza = o.AcikRiza,
                        CreatedAt = DateTime.SpecifyKind(o.CreatedAt, DateTimeKind.Utc)
                    })
                    .ToList();
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
