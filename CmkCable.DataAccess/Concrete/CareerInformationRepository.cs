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
            try
            {
                if (cvFile == null)
                {
                    Console.WriteLine("ConvertCvToBase64: CV file is null");
                    return null;
                }

                if (cvFile.Length == 0)
                {
                    Console.WriteLine("ConvertCvToBase64: CV file is empty");
                    return null;
                }

                Console.WriteLine($"Converting CV file: {cvFile.FileName}, Size: {cvFile.Length} bytes, ContentType: {cvFile.ContentType}");

                using (var memoryStream = new MemoryStream())
                {
                    cvFile.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    
                    if (fileBytes.Length == 0)
                    {
                        Console.WriteLine("ConvertCvToBase64: File bytes array is empty after copy");
                        return null;
                    }

                    var base64String = Convert.ToBase64String(fileBytes);
                    Console.WriteLine($"CV converted to base64 successfully, length: {base64String.Length}");
                    
                    return base64String;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting CV to base64: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Failed to convert CV to base64: {ex.Message}", ex);
            }
        }

        public CareerInformation CreateCareerInformation(CareerInformation careerInformation)
        {
            try
            {
                if (careerInformation == null)
                {
                    throw new ArgumentNullException(nameof(careerInformation), "Career information cannot be null");
                }

                using (var context = new CmkCableDbContext())
                {
                    Console.WriteLine($"Creating career information for: {careerInformation.FullName}");
                    Console.WriteLine($"Email: {careerInformation.Email}");
                    Console.WriteLine($"CV file: {careerInformation.Cv?.FileName ?? "None"}");

                    // Set default values if not provided
                    if (careerInformation.CreatedAt == default)
                    {
                        careerInformation.CreatedAt = DateTime.UtcNow;
                        Console.WriteLine($"Set CreatedAt to: {careerInformation.CreatedAt}");
                    }

                    // Process CV file if provided
                    if (careerInformation.Cv != null)
                    {
                        try
                        {
                            careerInformation.CvPath = ConvertCvToBase64(careerInformation.Cv);
                            Console.WriteLine($"CV converted to base64, length: {careerInformation.CvPath?.Length ?? 0}");
                        }
                        catch (Exception cvEx)
                        {
                            Console.WriteLine($"Failed to convert CV to base64: {cvEx.Message}");
                            // Continue without CV rather than failing completely
                            careerInformation.CvPath = null;
                        }
                    }

                    // Add to context
                    context.CareerInformations.Add(careerInformation);
                    Console.WriteLine("Career information added to context");

                    // Save changes
                    var result = context.SaveChanges();
                    Console.WriteLine($"Database save completed with {result} affected rows");

                    // Verify the entity was saved
                    if (careerInformation.Id <= 0)
                    {
                        Console.WriteLine("Warning: Career information ID was not set after save");
                    }
                    else
                    {
                        Console.WriteLine($"Career information saved successfully with ID: {careerInformation.Id}");
                    }

                    return careerInformation;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateCareerInformation: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Failed to create career information: {ex.Message}", ex);
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
                        Department = c.Department,
                        ReferenceSource = c.ReferenceSource,
                        Description = c.Description,
                        CvPath = c.CvPath,
                        Consent = c.Consent,
                        CreatedAt = DateTime.SpecifyKind(c.CreatedAt, DateTimeKind.Utc), // UTC olarak belirtiyoruz
                        IpAddress = c.IpAddress
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