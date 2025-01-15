using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IContactInformationService
    {
        List<ContactInformationDTO> GetAllContactInformations(int languageId);
        ContactInformation GetContactInformation(int id);
        ContactInformation CreateContactInformation(ContactInformation contactInformation);
        void DeleteContactInformation(int id);
        ContactInformation UpdateContactInformation(ContactInformation contactInformation);
    }
}
