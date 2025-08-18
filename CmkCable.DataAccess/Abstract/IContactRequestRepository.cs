using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface IContactRequestRepository
    {
        void Add(ContactRequest entity);
        void Delete(ContactRequest entity);
        List<ContactRequest> GetAllContactRequests();
        ContactRequest GetContactRequestById(int id);
        void Update(ContactRequest entity);
    }
}
