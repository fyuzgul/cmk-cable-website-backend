using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Abstract
{
    public interface IManagerMailService
    {
        List<ManagerMailDTO> GetAll();
        ManagerMail GetById(int id);
        List<ManagerMail> GetByType(string type);
        ManagerMail Add(ManagerMail managerMail, List<int> formTypeIds);
        ManagerMail Update(ManagerMail managerMail, List<FormType> formTypes);
        void Delete(int id);

    }
}
