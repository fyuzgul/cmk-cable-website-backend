using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface ICareerInformationRepository
    {
        List<CareerInformationDTO> GetAllCareerInformation();
        CareerInformation GetCareerInformationById(int id);
        CareerInformation CreateCareerInformation(CareerInformation careerInformation, List<Experience> experience);
        void DeleteCareerInformation(int id);
    }
}
