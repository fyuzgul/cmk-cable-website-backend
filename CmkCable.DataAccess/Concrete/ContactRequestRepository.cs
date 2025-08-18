using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class ContactRequestRepository : IContactRequestRepository
    {
        public void Add(ContactRequest entity)
        {
            using(var context = new CmkCableDbContext())
            {
                context.ContactRequests.Add(entity);
                context.SaveChanges();
            }
        }

        public void Delete(ContactRequest entity)
        {
            using(var context = new CmkCableDbContext())
            {
                context.ContactRequests.Remove(entity);
                context.SaveChanges();
            }
        }

        public List<ContactRequest> GetAllContactRequests()
        {
            using(var context = new CmkCableDbContext())
            {
                return context.ContactRequests
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();
            }
        }

        public ContactRequest GetContactRequestById(int id)
        {
            using(var context = new CmkCableDbContext())
            {
                return context.ContactRequests
                    .FirstOrDefault(c => c.Id == id);
            }
        }

        public void Update(ContactRequest entity)
        {
            using(var context = new CmkCableDbContext())
            {
                context.ContactRequests.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
