using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class ContactInfromationManager : IContactInformationService
    {
        private IContactInformationRepository _contactInformationRepository;

        public ContactInfromationManager()
        {
            _contactInformationRepository = new ContactInformationRepository();
        }

        public ContactInformationDetailDTO CreateContactInformation(ContactInformationCreateDTO dto)
        {
            return _contactInformationRepository.CreateContactInformation(dto);
        }

        public void DeleteContactInformation(int id)
        {
            _contactInformationRepository.DeleteContactInformation(id);
        }

        public List<ContactInformationDTO> GetAllContactInformations(int languageId)
        {
            return _contactInformationRepository.GetAllContactInformations(languageId);
        }

        public ContactInformationDetailDTO GetContactInformation(int id)
        {
            return _contactInformationRepository.GetContactInformation(id);
        }

        public ContactInformationDetailDTO UpdateContactInformation(ContactInformationCreateDTO dto, int id)
        {
            return _contactInformationRepository.UpdateContactInformation(dto, id);
        }

        public List<ContactInformationDetailDTO> GetAllContactInformationsWithTranslations()
        {
            return _contactInformationRepository.GetAllContactInformationsWithTranslations();
        }
    }
}
