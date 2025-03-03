using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        public ContactRequest CreateContactRequest(ContactRequest contactRequest)
        {
            using(var context = new CmkCableDbContext())
            {
                context.ContactRequests.Add(contactRequest);
                context.SaveChanges();
                return contactRequest;
            }
        }

        public void DeleteContactRequest(int id)
        {
            using(var context = new CmkCableDbContext())
            {
                var contactRequest = context.ContactRequests.Find(id);
                context.ContactRequests.Remove(contactRequest);
                context.SaveChanges();
            }
        }

        public List<ContactRequest> GetAllContactRequests()
        {
            using(var context = new CmkCableDbContext())
            {
                return context.ContactRequests.ToList();
            }
        }

        public ContactRequest GetContactRequestById(int id)
        {
            using(var context = new CmkCableDbContext())
            {
                return context.ContactRequests.Find(id);
            }
        }
    }
}
