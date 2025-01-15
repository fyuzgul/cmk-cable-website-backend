using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IContactInformationRepository
    {
        List<ContactInformationDTO> GetAllContactInformations(int languageId);
        ContactInformation GetContactInformation(int id);
        ContactInformation CreateContactInformation(ContactInformation contactInformation);    
        void DeleteContactInformation(int id);
        ContactInformation UpdateContactInformation(ContactInformation contactInformation);
    }
}
