using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class CompanyTypeRepository : ICompanyTypeRepository
    {
        public CompanyType CreateCompanyType(CompanyType companyType)
        {
            using (var context = new CmkCableDbContext())
            {
                context.CompanyTypes.Add(companyType);
                context.SaveChanges();
                return companyType;
            }
        }

        public CompanyType CreateCompanyTypeWithTranslations(CompanyType companyType, List<CompanyTypeTranslation> translations)
        {
            using (var context = new CmkCableDbContext())
            {
                using var tx = context.Database.BeginTransaction();
                try
                {
                    context.CompanyTypes.Add(companyType);
                    context.SaveChanges();

                    if (translations != null && translations.Count > 0)
                    {
                        foreach (var t in translations)
                        {
                            t.CompanyTypeId = companyType.Id;
                        }
                        context.CompanyTypeTranslations.AddRange(translations);
                        context.SaveChanges();
                    }

                    tx.Commit();
                    return companyType;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void DeleteCompanyType(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                var companyType = context.CompanyTypes.Find(id);
                if (companyType != null)
                {
                    context.CompanyTypes.Remove(companyType);
                    context.SaveChanges();
                }
            }
        }

        public List<CompanyType> GetAllCompanyTypes()
        {
            using (var context = new CmkCableDbContext())
            {
                return context.CompanyTypes
                    .Include(ct => ct.Translations)
                    .ToList();
            }
        }

        public List<CompanyType> GetActiveCompanyTypes()
        {
            using (var context = new CmkCableDbContext())
            {
                return context.CompanyTypes
                    .Where(ct => ct.IsActive)
                    .Include(ct => ct.Translations)
                    .ToList();
            }
        }

        public CompanyType GetCompanyTypeById(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                return context.CompanyTypes
                    .Include(ct => ct.Translations)
                    .FirstOrDefault(ct => ct.Id == id);
            }
        }

        public CompanyType UpdateCompanyType(CompanyType companyType)
        {
            using (var context = new CmkCableDbContext())
            {
                using var tx = context.Database.BeginTransaction();
                try
                {
                    var existingCompanyType = context.CompanyTypes
                        .Include(ct => ct.Translations)
                        .FirstOrDefault(ct => ct.Id == companyType.Id);
                        
                    if (existingCompanyType != null)
                    {
                        // Ana CompanyType'ı güncelle
                        existingCompanyType.Name = companyType.Name;
                        existingCompanyType.IsActive = companyType.IsActive;
                        
                        // Translations'ları güncelle
                        if (companyType.Translations != null && companyType.Translations.Count > 0)
                        {
                            // Mevcut translations'ları sil
                            context.CompanyTypeTranslations.RemoveRange(existingCompanyType.Translations);
                            
                            // Yeni translations'ları ekle
                            foreach (var translation in companyType.Translations)
                            {
                                translation.CompanyTypeId = companyType.Id;
                                context.CompanyTypeTranslations.Add(translation);
                            }
                        }
                        
                        context.SaveChanges();
                        tx.Commit();
                        
                        // Güncellenmiş CompanyType'ı translations ile birlikte döndür
                        return context.CompanyTypes
                            .Include(ct => ct.Translations)
                            .FirstOrDefault(ct => ct.Id == companyType.Id);
                    }
                    return null;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}

