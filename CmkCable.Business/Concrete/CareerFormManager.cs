using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class CareerFormManager : ICareerFormService
    {
        private ICareerFormRepository _repository;
        public CareerFormManager()
        {
            _repository = new CareerFormRepository();
        }
        public CareerForm CreateCareerForm(CareerForm careerForm)
        {
            return _repository.CreateCarrerForm(careerForm);
        }

        public CareerForm GetCareerFormById(int id)
        {
            return _repository.GetCarrerFormById(id);
        }
    }
}
