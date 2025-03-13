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
        List<ContactInformationDetailDTO> GetAllContactInformationsWithTranslations();
        ContactInformationDetailDTO GetContactInformation(int id);
        ContactInformationDetailDTO CreateContactInformation(ContactInformationCreateDTO dto);
        void DeleteContactInformation(int id);
        ContactInformationDetailDTO UpdateContactInformation(ContactInformationCreateDTO dto, int id);
    }
}
