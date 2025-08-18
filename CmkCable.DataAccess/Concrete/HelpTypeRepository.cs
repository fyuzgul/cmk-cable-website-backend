using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class HelpTypeRepository : IHelpTypeRepository
    {
        public HelpType CreateHelpType(HelpType helpType)
        {
            using (var context = new CmkCableDbContext())
            {
                context.HelpTypes.Add(helpType);
                context.SaveChanges();
                return helpType;
            }
        }

        public HelpType CreateHelpTypeWithTranslations(HelpType helpType, List<HelpTypeTranslation> translations)
        {
            using (var context = new CmkCableDbContext())
            {
                using var tx = context.Database.BeginTransaction();
                try
                {
                    context.HelpTypes.Add(helpType);
                    context.SaveChanges();

                    if (translations != null && translations.Count > 0)
                    {
                        foreach (var t in translations)
                        {
                            t.HelpTypeId = helpType.Id;
                        }
                        context.HelpTypeTranslations.AddRange(translations);
                        context.SaveChanges();
                    }

                    tx.Commit();
                    return helpType;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void DeleteHelpType(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                var helpType = context.HelpTypes.Find(id);
                if (helpType != null)
                {
                    context.HelpTypes.Remove(helpType);
                    context.SaveChanges();
                }
            }
        }

        public List<HelpType> GetAllHelpTypes()
        {
            using (var context = new CmkCableDbContext())
            {
                return context.HelpTypes
                    .Include(ht => ht.Translations)
                    .ToList();
            }
        }

        public List<HelpType> GetActiveHelpTypes()
        {
            using (var context = new CmkCableDbContext())
            {
                return context.HelpTypes
                    .Where(ht => ht.IsActive)
                    .Include(ht => ht.Translations)
                    .ToList();
            }
        }

        public HelpType GetHelpTypeById(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                return context.HelpTypes
                    .Include(ht => ht.Translations)
                    .FirstOrDefault(ht => ht.Id == id);
            }
        }

        public HelpType UpdateHelpType(HelpType helpType)
        {
            using (var context = new CmkCableDbContext())
            {
                using var tx = context.Database.BeginTransaction();
                try
                {
                    var existingHelpType = context.HelpTypes
                        .Include(ht => ht.Translations)
                        .FirstOrDefault(ht => ht.Id == helpType.Id);
                        
                    if (existingHelpType != null)
                    {
                        // Ana HelpType'ı güncelle
                        existingHelpType.Name = helpType.Name;
                        existingHelpType.IsActive = helpType.IsActive;
                        
                        // Translations'ları güncelle
                        if (helpType.Translations != null && helpType.Translations.Count > 0)
                        {
                            // Mevcut translations'ları sil
                            context.HelpTypeTranslations.RemoveRange(existingHelpType.Translations);
                            
                            // Yeni translations'ları ekle
                            foreach (var translation in helpType.Translations)
                            {
                                translation.HelpTypeId = helpType.Id;
                                context.HelpTypeTranslations.Add(translation);
                            }
                        }
                        
                        context.SaveChanges();
                        tx.Commit();
                        
                        // Güncellenmiş HelpType'ı translations ile birlikte döndür
                        return context.HelpTypes
                            .Include(ht => ht.Translations)
                            .FirstOrDefault(ht => ht.Id == helpType.Id);
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

