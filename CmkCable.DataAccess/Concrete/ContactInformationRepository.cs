using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
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

        public List<ContactInformation> GetAllContactInformations()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.ContactInformations.ToList();
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
