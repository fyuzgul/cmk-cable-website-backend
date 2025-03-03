using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class FormTypesRepository : IFormTypesRepository
    {
        public List<FormType> GetAll()
        {
            using(var context = new CmkCableDbContext())
            {
                return context.FormTypes.ToList();
            }
        }
    }
}
