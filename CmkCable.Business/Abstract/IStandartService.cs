using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IStandartService
    {
        List<Standart> GetAllStandart();
        List<Standart> GetStandartsByProducId(int productId);
        List<Standart> GetStandartsByCertificateId(int certificateId);
        Standart GetStandartById(int id);


        void DeleteStandart(int id);
        Standart CreateStandart(Standart standart);
        Standart UpdateStandart(Standart standart);

    }
}
