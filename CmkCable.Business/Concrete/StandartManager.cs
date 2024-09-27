using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class StandartManager : IStandartService
    {
        private IStandartRepository _standartRepository;
        public StandartManager() { _standartRepository = new StandartRepository(); }

        public Standart CreateStandart(Standart standart)
        {
            return _standartRepository.CreateStandart(standart);
        }

        public void DeleteStandart(int id)
        {
            _standartRepository.DeleteStandart(id);
        }

        public List<Standart> GetAllStandart()
        {
            return _standartRepository.GetAllStandart();
        }

        public Standart GetStandartById(int id)
        {
            return _standartRepository.GetStandartById(id);
        }

        public List<Standart> GetStandartsByCertificateId(int certificateId)
        {
            return _standartRepository.GetStandartsByCertificateId(certificateId);
        }

        public List<Standart> GetStandartsByProducId(int productId)
        {
            return _standartRepository.GetStandartsByProducId(productId);
        }

        public Standart UpdateStandart(Standart standart)
        {
            return _standartRepository.UpdateStandart(standart);
        }
    }
}
