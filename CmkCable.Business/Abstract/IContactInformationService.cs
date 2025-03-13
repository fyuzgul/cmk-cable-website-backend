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
        List<ContactInformationDetailDTO> GetAllContactInformationsWithTranslations();
        ContactInformationDetailDTO GetContactInformation(int id);
        ContactInformationDetailDTO CreateContactInformation(ContactInformationCreateDTO dto);
        void DeleteContactInformation(int id);
        ContactInformationDetailDTO UpdateContactInformation(ContactInformationCreateDTO dto, int id);
    }
}
