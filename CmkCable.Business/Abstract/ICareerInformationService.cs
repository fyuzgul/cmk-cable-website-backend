using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Abstract
{
    public interface ICareerInformationService
    {
        List<CareerInformationDTO> GetAllCareerInformation();
        CareerInformation GetCareerInformationById(int id);
        CareerInformation CreateCareerInformation(CareerInformation careerInformation);
        void DeleteCareerInformation(int id);
    }
}
