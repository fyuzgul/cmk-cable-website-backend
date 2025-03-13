using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class CareerInformationRepository : ICareerInformationRepository
    {
        public string ConvertCvToBase64(IFormFile cvFile)
        {
            if (cvFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    cvFile.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(fileBytes);
                }
            }
            return null;
        }

        public CareerInformation CreateCareerInformation(CareerInformation careerInformation, List<Experience> experience)
        {
            using (var context = new CmkCableDbContext())
            {
                careerInformation.CvPath = ConvertCvToBase64(careerInformation.Cv);

                context.CareerInformations.Add(careerInformation);
                context.SaveChanges(); // ID burada oluşuyor

                foreach (var exp in experience)
                {
                    exp.CareerInformationId = careerInformation.Id;
                    context.Experiences.Add(exp);
                }

                context.SaveChanges();

                return careerInformation;
            }
        }



        public void DeleteCareerInformation(int id)
        {
            using (var dbContext = new CmkCableDbContext())
            {
                var career = dbContext.CareerInformations.Find(id);
                dbContext.CareerInformations.Remove(career);
                dbContext.SaveChanges();
            }
        }

        public List<CareerInformationDTO> GetAllCareerInformation()
        {
            using (var context = new CmkCableDbContext())
            {
                var careerInformations = context.CareerInformations
                    .Include(c => c.Experiences)
                    .OrderByDescending(c => c.CreatedAt) // En son eklenenler üstte olacak
                    .Select(c => new CareerInformationDTO
                    {
                        Id = c.Id,
                        FullName = c.FullName,
                        TelephoneNumber = c.TelephoneNumber,
                        Email = c.Email,
                        Gender = c.Gender,
                        MaritalStatus = c.MaritalStatus,
                        MilitaryStatus = c.MilitaryStatus,
                        DriverLicense = c.DriverLicense,
                        TravelAvailability = c.TravelAvailability,
                        School = c.School,
                        Faculty = c.Faculty,
                        GraduationDate = c.GraduationDate,
                        Languages = c.Languages,
                        SoftwareSkills = c.SoftwareSkills,
                        Seminars = c.Seminars,
                        Department = c.Department,
                        ReferenceSource = c.ReferenceSource,
                        Description = c.Description,
                        CvPath = c.CvPath,
                        Consent = c.Consent,
                        CreatedAt = DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc) // UTC olarak belirtiyoruz
                    })
                    .ToList();

                return careerInformations;
            }
        }



        public CareerInformation GetCareerInformationById(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                return context.CareerInformations.Find(id);
            }
        }
    }
}