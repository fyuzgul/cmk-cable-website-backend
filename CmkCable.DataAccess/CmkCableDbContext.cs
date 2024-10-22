using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using CmkCable.Entities.CmkCable.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace CmkCable.DataAccess
{
    public class CmkCableDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-HT2540H;Database=CmkCableDb;Integrated Security=True;");
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Standart> Standarts { get; set; }
        public DbSet<Structure> Structures { get; set; }
        public DbSet<TechnicalFeature> TechnicalFeatures { get; set; }
        public DbSet<CertificateType> CertificateTypes { get; set; }
        public DbSet<CareerForm> CareerForms { get; set; }  
        public DbSet<ContactInformation> ContactInformations { get; set; }
        public DbSet<HistoryItem> HistoryItems { get; set; }
        public DbSet<AboutUsItem> AboutUsItems { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<NavbarItem> NavbarItems { get; set; }
        public DbSet<NavbarItemTranslation> NavbarItemsTranslations { get; set; }
        public DbSet<ProductCertificate> ProductCertificates { get; set; }  
        public DbSet<ProductStandart> ProductStandarts { get; set; }    
        public DbSet<ProductStructure> ProductStructures { get; set; }  
        public DbSet<CertificateStandart> CertificateStandarts { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }


    }
}