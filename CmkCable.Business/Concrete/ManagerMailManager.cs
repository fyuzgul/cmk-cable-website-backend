using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Concrete
{
    public class ManagerMailManager : IManagerMailService
    {
        private ManagerMailRepository _managerMailRepository;
        public ManagerMailManager()
        {
            _managerMailRepository = new ManagerMailRepository();
        }
        public ManagerMail Add(ManagerMail managerMail, List<int> formTypeIds)
        {
            return _managerMailRepository.Add(managerMail, formTypeIds);
        }

        public void Delete(int id)
        {
            _managerMailRepository.Delete(id);
        }

        public List<ManagerMailDTO> GetAll()
        {
           return _managerMailRepository.GetAll();
        }

        public ManagerMail GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ManagerMail> GetByType(string type)
        {
            throw new NotImplementedException();
        }

        public ManagerMail Update(ManagerMail managerMail, List<FormType> formTypes)
        {
            return _managerMailRepository.Update(managerMail, formTypes);
        }
    }
}
