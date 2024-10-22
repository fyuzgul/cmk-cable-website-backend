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
            throw new NotImplementedException();
        }

        public void DeleteStandart(int id)
        {
            throw new NotImplementedException();
        }

        public List<Standart> GetAllStandart()
        {
            throw new NotImplementedException();
        }

        public Standart GetStandartById(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
