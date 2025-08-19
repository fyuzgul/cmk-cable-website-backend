using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs.CreateDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetOffersController : ControllerBase
    {
        private readonly GetOfferManager _getOfferService;
        private readonly EmailManager _emailManager;

        public GetOffersController()
        {
            _getOfferService = new GetOfferManager();
            _emailManager = new EmailManager();
        }

        [HttpGet]
        public IActionResult GetAllGetOffers()
        {
            var offers = _getOfferService.GetAllGetOffers();
            return Ok(offers);
        }

        [HttpGet("get/{id}")]
        public IActionResult Get(int id)
        {
            var offer = _getOfferService.GetOfferById(id);
            if (offer == null)
                return NotFound();
            return Ok(offer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateGetOfferDTO createGetOfferDTO)
        {
            var getOffer = new GetOffer
            {
                FirstName = createGetOfferDTO.FirstName,
                LastName = createGetOfferDTO.LastName,
                WorkEmail = createGetOfferDTO.WorkEmail,
                RoleId = createGetOfferDTO.RoleId,
                Country = createGetOfferDTO.Country,
                Company = createGetOfferDTO.Company,
                CompanyTypeId = createGetOfferDTO.CompanyTypeId,
                TelephoneNumber = createGetOfferDTO.TelephoneNumber,
                HelpTypeId = createGetOfferDTO.HelpTypeId,
                Message = createGetOfferDTO.Message,
                IpAddress = createGetOfferDTO.IpAddress,
                AcikRiza = createGetOfferDTO.AcikRiza
            };

            var result = _getOfferService.CreateGetOffer(getOffer);
            
            // Mail gönder
            try
            {
                Console.WriteLine($"Mail gönderimi başlıyor... GetOffer ID: {result.Id}");
                
                // GetOffer'ı navigation property'ler ile yükle
                var offerWithDetails = _getOfferService.GetOfferById(result.Id);
                
                await _emailManager.SendOfferEmailAsync("Yeni Teklif Talebi", offerWithDetails);
                Console.WriteLine("Mail gönderimi tamamlandı!");
            }
            catch (Exception ex)
            {
                // Mail gönderim hatası loglansın ama işlem devam etsin
                Console.WriteLine($"Mail gönderim hatası: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            
            return Ok(result);
        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _getOfferService.DeleteGetOffer(id);
            return NoContent();
        }

        [HttpGet("dropdowns")]
        public IActionResult GetDropdownData()
        {
            var roleService = new RoleManager();
            var companyTypeService = new CompanyTypeManager();
            var helpTypeService = new HelpTypeManager();

            var dropdownData = new
            {
                roles = roleService.GetActiveRoles().Select(r => new { 
                    Id = r.Id, 
                    Name = r.Name,
                    Translations = r.Translations?.Select(t => new { 
                        LanguageId = t.LanguageId, 
                        Name = t.Name 
                    }).ToList()
                }),
                companyTypes = companyTypeService.GetActiveCompanyTypes().Select(ct => new { 
                    Id = ct.Id, 
                    Name = ct.Name,
                    Translations = ct.Translations?.Select(t => new { 
                        LanguageId = t.LanguageId, 
                        Name = t.Name 
                    }).ToList()
                }),
                helpTypes = helpTypeService.GetActiveHelpTypes().Select(ht => new { 
                    Id = ht.Id, 
                    Name = ht.Name,
                    Translations = ht.Translations?.Select(t => new { 
                        LanguageId = t.LanguageId, 
                        Name = t.Name 
                    }).ToList()
                })
            };

            return Ok(dropdownData);
        }
    }
}
