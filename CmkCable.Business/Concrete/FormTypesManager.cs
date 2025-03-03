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
    public class FormTypesManager : IFormTypesService
    {
        private IFormTypesRepository formTypesRepository;
        public FormTypesManager()
        {
            formTypesRepository = new FormTypesRepository();
        }

        public List<FormType> GetAll()
        {
            return formTypesRepository.GetAll();
        }
    }
}
