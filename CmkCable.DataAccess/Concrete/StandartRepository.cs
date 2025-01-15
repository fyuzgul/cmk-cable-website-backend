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
                var standart = cmkCableDbContext.Standarts.Find(id);
                if (standart != null)
                {
                    cmkCableDbContext.Standarts.Remove(standart);
                    cmkCableDbContext.SaveChanges();
                }
                else
                {
                    throw new KeyNotFoundException("Standart not found");
                }
            }
        }

        public List<Standart> GetAllStandart()
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) { return cmkCableDbContext.Standarts.ToList(); }
        }

        public Standart GetStandartById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext()) { return cmkCableDbContext.Standarts.Find(id); }
        }

        public List<Standart> GetStandartsByCertificateId(int certificateId)
        {
            throw new NotImplementedException();
        }

        public List<Standart> GetStandartsByProducId(int productId)
        {
            throw new NotImplementedException();
        }

        public Standart UpdateStandart(Standart standart)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var existingStandart = cmkCableDbContext.Standarts.Find(standart.Id);
                if (existingStandart != null)
                {
                    existingStandart.Name = standart.Name;

                    cmkCableDbContext.SaveChanges();
                    return existingStandart;
                }
                else
                {
                    throw new KeyNotFoundException("Standart not found");
                }
            }
        }
    }
}
