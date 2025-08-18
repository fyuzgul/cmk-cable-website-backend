using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class RoleRepository : IRoleRepository
    {
        public Role CreateRole(Role role)
        {
            using (var context = new CmkCableDbContext())
            {
                context.Roles.Add(role);
                context.SaveChanges();
                return role;
            }
        }

        public Role CreateRoleWithTranslations(Role role, List<RoleTranslation> translations)
        {
            using (var context = new CmkCableDbContext())
            {
                using var tx = context.Database.BeginTransaction();
                try
                {
                    context.Roles.Add(role);
                    context.SaveChanges();

                    if (translations != null && translations.Count > 0)
                    {
                        foreach (var t in translations)
                        {
                            t.RoleId = role.Id;
                        }
                        context.RoleTranslations.AddRange(translations);
                        context.SaveChanges();
                    }

                    tx.Commit();
                    return role;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void DeleteRole(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                var role = context.Roles.Find(id);
                if (role != null)
                {
                    context.Roles.Remove(role);
                    context.SaveChanges();
                }
            }
        }

        public List<Role> GetAllRoles()
        {
            using (var context = new CmkCableDbContext())
            {
                return context.Roles
                    .Include(r => r.Translations)
                    .ToList();
            }
        }

        public List<Role> GetActiveRoles()
        {
            using (var context = new CmkCableDbContext())
            {
                return context.Roles
                    .Where(r => r.IsActive)
                    .Include(r => r.Translations)
                    .ToList();
            }
        }

        public Role GetRoleById(int id)
        {
            using (var context = new CmkCableDbContext())
            {
                return context.Roles
                    .Include(r => r.Translations)
                    .FirstOrDefault(r => r.Id == id);
            }
        }

        public Role UpdateRole(Role role)
        {
            using (var context = new CmkCableDbContext())
            {
                using var tx = context.Database.BeginTransaction();
                try
                {
                    var existingRole = context.Roles
                        .Include(r => r.Translations)
                        .FirstOrDefault(r => r.Id == role.Id);
                        
                    if (existingRole != null)
                    {
                        // Ana Role'ü güncelle
                        existingRole.Name = role.Name;
                        existingRole.IsActive = role.IsActive;
                        
                        // Translations'ları güncelle
                        if (role.Translations != null && role.Translations.Count > 0)
                        {
                            // Mevcut translations'ları sil
                            context.RoleTranslations.RemoveRange(existingRole.Translations);
                            
                            // Yeni translations'ları ekle
                            foreach (var translation in role.Translations)
                            {
                                translation.RoleId = role.Id;
                                context.RoleTranslations.Add(translation);
                            }
                        }
                        
                        context.SaveChanges();
                        tx.Commit();
                        
                        // Güncellenmiş Role'ü translations ile birlikte döndür
                        return context.Roles
                            .Include(r => r.Translations)
                            .FirstOrDefault(r => r.Id == role.Id);
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

