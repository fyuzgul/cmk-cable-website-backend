using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class StandartRepository : IStandartRepository
    {
        public Standart CreateStandart(Standart standart)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Standarts.Add(standart);
                cmkCableDbContext.SaveChanges();
                return standart;
            }
        }

        public void DeleteStandart(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedStandart = cmkCableDbContext.Standarts.Find(id);
                cmkCableDbContext.Standarts.Remove(deletedStandart);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<Standart> GetAllStandart()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Standarts.ToList();
            }
        }

        public Standart GetStandartById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Standarts.Find(id);
            }
        }

        public List<Standart> GetStandartsByCertificateId(int certificateId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Standarts.Where(p => p.StandartCertificates.Any(ps => ps.CertificateId == certificateId))
                    .ToList();
            }
        }

        public List<Standart> GetStandartsByProducId(int productId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Standarts.Where(p => p.ProductStandarts.Any(ps => ps.ProductId == productId))
                    .ToList();
            }
        }

        public Standart UpdateStandart(Standart standart)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Standarts.Update(standart);
                cmkCableDbContext.SaveChanges();
                return standart;
            }
        }
    }
}
