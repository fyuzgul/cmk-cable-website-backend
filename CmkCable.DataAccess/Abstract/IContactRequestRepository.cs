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
        List<ContactRequest> GetAllContactRequests();
        ContactRequest GetContactRequestById(int id);
        ContactRequest CreateContactRequest(ContactRequest contactRequest);
        void DeleteContactRequest(int id);
    }
}
