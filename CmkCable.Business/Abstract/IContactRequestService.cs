using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Abstract
{
    public interface IContactRequestService
    {
        List<ContactRequest> GetAllContactRequests();
        ContactRequest GetContactRequestById(int id);
        ContactRequest CreateContactRequest(ContactRequest contactRequest);
        void DeleteContactRequest(int id);
    }
}
