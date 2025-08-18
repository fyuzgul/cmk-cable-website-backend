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
               _contactRequestRepository.Add(contactRequest);
               return contactRequest;   
        }

        public void DeleteContactRequest(int id)
        {
            var contactRequest = _contactRequestRepository.GetContactRequestById(id);
            if (contactRequest != null)
            {
                _contactRequestRepository.Delete(contactRequest);
            }
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
