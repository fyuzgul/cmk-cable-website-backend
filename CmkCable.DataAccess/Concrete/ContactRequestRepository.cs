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
                return context.ContactRequests
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new ContactRequest
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        Street = c.Street,
                        City = c.City,
                        IpAddress = c.IpAddress,
                        Postcode = c.Postcode,
                        TelephoneNumber = c.TelephoneNumber,
                        Email = c.Email,
                        Message = c.Message,
                        Consent = c.Consent,
                        CreatedAt = DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc)
                    })
                    .ToList();
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
