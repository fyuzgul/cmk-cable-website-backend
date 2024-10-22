using CmkCable.Entities;


namespace CmkCable.DataAccess.Abstract
{
    public interface ICareerFormRepository
    {
        CareerForm GetCarrerFormById(int id);
        CareerForm CreateCarrerForm(CareerForm careerForm);
    }
}
