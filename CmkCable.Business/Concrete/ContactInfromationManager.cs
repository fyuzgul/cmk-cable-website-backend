using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class ContactInfromationManager : IContactInformationService
    {
        private IContactInformationRepository _contactInformationRepository;

        public ContactInfromationManager ()
        {
            _contactInformationRepository = new ContactInformationRepository();
        }
        public ContactInformation CreateContactInformation(ContactInformation contactInformation)
        {
            return _contactInformationRepository.CreateContactInformation(contactInformation);
        }

        public void DeleteContactInformation(int id)
        {
            _contactInformationRepository.DeleteContactInformation(id);
        }

        public List<ContactInformation> GetAllContactInformations()
        {
            return _contactInformationRepository.GetAllContactInformations();
        }

        public ContactInformation GetContactInformation(int id)
        {
            return _contactInformationRepository.GetContactInformation(id);
        }

        public ContactInformation UpdateContactInformation(ContactInformation contactInformation)
        {
           return _contactInformationRepository.UpdateContactInformation(contactInformation);   
        }
    }
}
