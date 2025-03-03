using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Concrete
{
    public class ContactRequestManager : IContactRequestService
    {
        private IContactRequestRepository _contactRequestRepository;
        public ContactRequestManager()
        {
            _contactRequestRepository = new ContactRequestRepository();
        }

        public ContactRequest CreateContactRequest(ContactRequest contactRequest)
        {
               return _contactRequestRepository.CreateContactRequest(contactRequest);   
        }

        public void DeleteContactRequest(int id)
        {
            _contactRequestRepository.DeleteContactRequest(id);
        }

        public List<ContactRequest> GetAllContactRequests()
        {
            return _contactRequestRepository.GetAllContactRequests();
        }

        public ContactRequest GetContactRequestById(int id)
        {
            return _contactRequestRepository.GetContactRequestById(id);
        }
    }
}
