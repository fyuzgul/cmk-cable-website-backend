using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class CareerFormRepository : ICareerFormRepository
    {
         public CareerForm CreateCarrerForm(CareerForm careerForm)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.CareerForms.Add(careerForm);
                cmkCableDbContext.SaveChanges();
                return careerForm;
            }
        }

        public CareerForm GetCarrerFormById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
               return cmkCableDbContext.CareerForms.Find(id);
            }
        }
    }
}
