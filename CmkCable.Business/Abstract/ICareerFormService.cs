using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface ICareerFormService
    {
        CareerForm GetCareerFormById(int id);
        CareerForm CreateCareerForm(CareerForm careerForm);
    }
}
