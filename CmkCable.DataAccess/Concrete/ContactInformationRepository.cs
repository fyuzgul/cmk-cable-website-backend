using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class ContactInformationRepository : IContactInformationRepository
    {
        public ContactInformationDetailDTO CreateContactInformation(ContactInformationCreateDTO dto)
        {
            try
            {
                using (var context = new CmkCableDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var contactInformation = new ContactInformation
                            {
                                PhoneNumber = dto.PhoneNumber?.Trim(),
                                Email = dto.Email?.Trim(),
                                FaxNumber = dto.FaxNumber?.Trim()
                            };

                            context.ContactInformations.Add(contactInformation);
                            context.SaveChanges();

                            if (dto.Translations != null && dto.Translations.Any())
                            {
                                foreach (var translation in dto.Translations)
                                {
                                    // Dil ID'sinin geçerli olduğunu kontrol et
                                    var languageExists = context.Languages.Any(l => l.Id == translation.LanguageId);
                                    if (!languageExists)
                                    {
                                        throw new Exception($"Geçersiz dil ID: {translation.LanguageId}");
                                    }

                                    var contactTranslation = new ContactInformationTranslation
                                    {
                                        ContactInformationId = contactInformation.Id,
                                        LanguageId = translation.LanguageId,
                                        Department = translation.Department?.Trim()
                                    };
                                    context.ContactInformationTranslations.Add(contactTranslation);
                                }
                                context.SaveChanges();
                            }

                            transaction.Commit();

                            // Yeni oluşturulan veriyi getir
                            var translations = context.ContactInformationTranslations
                                .Where(t => t.ContactInformationId == contactInformation.Id)
                                .AsNoTracking()
                                .Select(t => new TranslationDTO
                                {
                                    LanguageId = t.LanguageId,
                                    Department = t.Department
                                })
                                .ToList();
                            
                            return new ContactInformationDetailDTO
                            {
                                Id = contactInformation.Id,
                                PhoneNumber = contactInformation.PhoneNumber,
                                Email = contactInformation.Email,
                                FaxNumber = contactInformation.FaxNumber,
                                Translations = translations
                            };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("İletişim bilgisi kaydedilirken bir hata oluştu: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("İletişim bilgisi işlemi başarısız: " + ex.Message);
            }
        }

        public void DeleteContactInformation(int id)
        {
            try
            {
                using (var context = new CmkCableDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Önce çevirileri sil
                            var translationsToRemove = context.ContactInformationTranslations
                                .Where(t => t.ContactInformationId == id);
                            context.ContactInformationTranslations.RemoveRange(translationsToRemove);

                            // Sonra ana kaydı sil
                            var contactInformation = context.ContactInformations
                                .FirstOrDefault(c => c.Id == id);

                            if (contactInformation == null)
                                throw new Exception($"ID {id} olan iletişim bilgisi bulunamadı.");
                            
                            context.ContactInformations.Remove(contactInformation);
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("İletişim bilgisi silinirken bir hata oluştu: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Silme işlemi başarısız: " + ex.Message);
            }
        }

        public List<ContactInformationDTO> GetAllContactInformations(int languageId)
        {
            try
            {
                using (var context = new CmkCableDbContext())
                {
                    // Önce dil ID'sinin geçerli olduğunu kontrol et
                    var languageExists = context.Languages.Any(l => l.Id == languageId);
                    if (!languageExists)
                    {
                        throw new Exception($"Geçersiz dil ID: {languageId}");
                    }

                    return context.ContactInformations
                        .AsNoTracking()
                        .Join(context.ContactInformationTranslations,
                            ci => ci.Id, 
                            cit => cit.ContactInformationId,
                            (ci, cit) => new { ci, cit })
                        .Where(x => x.cit.LanguageId == languageId) 
                        .Select(x => new ContactInformationDTO
                        {
                            Id = x.ci.Id,
                            PhoneNumber = x.ci.PhoneNumber,
                            Email = x.ci.Email,
                            FaxNumber = x.ci.FaxNumber,
                            Department = x.cit.Department
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("İletişim bilgileri getirilirken bir hata oluştu: " + ex.Message);
            }
        }

        public ContactInformationDetailDTO GetContactInformation(int id)
        {
            try
            {
                using (var context = new CmkCableDbContext())
                {
                    var contactInformation = context.ContactInformations
                        .Include(c => c.Translations)
                        .AsNoTracking()
                        .FirstOrDefault(c => c.Id == id);

                    if (contactInformation == null)
                        throw new Exception($"ID {id} olan iletişim bilgisi bulunamadı.");

                    return new ContactInformationDetailDTO
                    {
                        Id = contactInformation.Id,
                        PhoneNumber = contactInformation.PhoneNumber,
                        Email = contactInformation.Email,
                        FaxNumber = contactInformation.FaxNumber,
                        Translations = contactInformation.Translations.Select(t => new TranslationDTO
                        {
                            LanguageId = t.LanguageId,
                            Department = t.Department
                        }).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("İletişim bilgisi getirilirken bir hata oluştu: " + ex.Message);
            }
        }

        public ContactInformationDetailDTO UpdateContactInformation(ContactInformationCreateDTO dto, int id)
        {
            try
            {
                using (var context = new CmkCableDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var contactInformation = context.ContactInformations
                                .Include(c => c.Translations)
                                .FirstOrDefault(c => c.Id == id);

                            if (contactInformation == null)
                                throw new Exception($"ID {id} olan iletişim bilgisi bulunamadı.");

                            contactInformation.PhoneNumber = dto.PhoneNumber?.Trim();
                            contactInformation.Email = dto.Email?.Trim();
                            contactInformation.FaxNumber = dto.FaxNumber?.Trim();

                            // Mevcut çevirileri sil
                            var translationsToRemove = context.ContactInformationTranslations
                                .Where(t => t.ContactInformationId == id);
                            context.ContactInformationTranslations.RemoveRange(translationsToRemove);

                            if (dto.Translations != null && dto.Translations.Any())
                            {
                                foreach (var translation in dto.Translations)
                                {
                                    // Dil ID'sinin geçerli olduğunu kontrol et
                                    var languageExists = context.Languages.Any(l => l.Id == translation.LanguageId);
                                    if (!languageExists)
                                    {
                                        throw new Exception($"Geçersiz dil ID: {translation.LanguageId}");
                                    }

                                    var contactTranslation = new ContactInformationTranslation
                                    {
                                        ContactInformationId = contactInformation.Id,
                                        LanguageId = translation.LanguageId,
                                        Department = translation.Department?.Trim()
                                    };
                                    context.ContactInformationTranslations.Add(contactTranslation);
                                }
                            }

                            context.SaveChanges();
                            transaction.Commit();

                            // Güncellenmiş veriyi getir
                            var updatedTranslations = context.ContactInformationTranslations
                                .Where(t => t.ContactInformationId == id)
                                .AsNoTracking()
                                .Select(t => new TranslationDTO
                                {
                                    LanguageId = t.LanguageId,
                                    Department = t.Department
                                })
                                .ToList();

                            return new ContactInformationDetailDTO
                            {
                                Id = contactInformation.Id,
                                PhoneNumber = contactInformation.PhoneNumber,
                                Email = contactInformation.Email,
                                FaxNumber = contactInformation.FaxNumber,
                                Translations = updatedTranslations
                            };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("İletişim bilgisi güncellenirken bir hata oluştu: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Güncelleme işlemi başarısız: " + ex.Message);
            }
        }

        public List<ContactInformationDetailDTO> GetAllContactInformationsWithTranslations()
        {
            try
            {
                using (var context = new CmkCableDbContext())
                {
                    return context.ContactInformations
                        .Include(c => c.Translations)
                        .AsNoTracking()
                        .Select(c => new ContactInformationDetailDTO
                        {
                            Id = c.Id,
                            PhoneNumber = c.PhoneNumber,
                            Email = c.Email,
                            FaxNumber = c.FaxNumber,
                            Translations = c.Translations.Select(t => new TranslationDTO
                            {
                                LanguageId = t.LanguageId,
                                Department = t.Department
                            }).ToList()
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("İletişim bilgileri getirilirken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
