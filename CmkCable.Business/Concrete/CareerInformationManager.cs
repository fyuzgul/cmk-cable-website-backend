using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using System.Collections.Generic;

namespace CmkCable.Business.Concrete
{
    public class CareerInformationManager : ICareerInformationService
    {
        private readonly ICareerInformationRepository _careerInformationRepository;
        public CareerInformationManager()
        {
            _careerInformationRepository = new CareerInformationRepository();
        }

        public CareerInformation CreateCareerInformation(CareerInformation careerInformation, List<Experience> experience)
        {
               return _careerInformationRepository.CreateCareerInformation(careerInformation, experience);
        }

        public void DeleteCareerInformation(int id)
        {
            _careerInformationRepository.DeleteCareerInformation(id);
        }

        public List<CareerInformationDTO> GetAllCareerInformation()
        {
            return _careerInformationRepository.GetAllCareerInformation();
        }

        public CareerInformation GetCareerInformationById(int id)
        {
            return _careerInformationRepository.GetCareerInformationById(id);
        }
    }
}
