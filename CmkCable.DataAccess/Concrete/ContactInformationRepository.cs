using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ContactInformationRepository : IContactInformationRepository
    {
        public ContactInformation CreateContactInformation(ContactInformation contactInformation)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.ContactInformations.Add(contactInformation);
                cmkCableDbContext.SaveChanges();
                return contactInformation;
            }
        }

        public void DeleteContactInformation(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedInformation = cmkCableDbContext.ContactInformations.Find(id);
                cmkCableDbContext.ContactInformations.Remove(deletedInformation);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<ContactInformationDTO> GetAllContactInformations(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.ContactInformations
                    .Join(cmkCableDbContext.ContactInformationTranslations,
                        ci => ci.Id, 
                        cit => cit.ContactInformationId,
                        (ci, cit) => new { ci, cit })
                    .Where(x => x.cit.LanguageId == languageId) 
                    .Select(x => new ContactInformationDTO
                    {
                        PhoneNumber = x.ci.PhoneNumber,
                        Email = x.ci.Email,
                        FaxNumber = x.ci.FaxNumber,
                        Department = x.cit.Department
                    })
                    .ToList();
            }
        }



        public ContactInformation GetContactInformation(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.ContactInformations.Find(id);
            }
        }

        public ContactInformation UpdateContactInformation(ContactInformation contactInformation)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.ContactInformations.Update(contactInformation);
                cmkCableDbContext.SaveChanges();
                return contactInformation;
            }
        }
    }
}
