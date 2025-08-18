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

            // Local PostgreSQL 17 database connection
            optionsBuilder.UseNpgsql("Host=165.22.95.181;Port=5432;Database=CmkCableProd;Username=postgres;Password=20analyzer16");

        }


        


        public DbSet<Category> Categories { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Standart> Standarts { get; set; }
        public DbSet<Structure> Structures { get; set; }
        public DbSet<TechnicalFeature> TechnicalFeatures { get; set; }
        public DbSet<CertificateType> CertificateTypes { get; set; }
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
        public DbSet<HomePageText> HomePageTexts { get; set; }  
        public DbSet<HomePageTextTranslation> HomePageTextTranslations { get; set; }
        public DbSet<PdsDocument> PdsDocuments { get; set; }
        public DbSet<PdsDocumentTranslation> PdsDocumentsTranslations { get; set; }
        public DbSet<ContactInformationTranslation> ContactInformationTranslations { get; set; }
        public DbSet<HistoryItemTranslation> HistoryItemTranslations { get; set; }  
        public DbSet<StructureTranslation> StructureTranslations { get; set; }
        public DbSet<User> Users { get; set; } 
        public DbSet<SwiperItem> SwiperItems { get; set; }
        public DbSet<GetOffer> GetOffers { get; set; }
        public DbSet<ContactRequest> ContactRequests { get; set; }
        public DbSet<CareerInformation> CareerInformations { get; set; }
        public DbSet<ManagerMail> ManagerMails { get; set; }
        public DbSet<MailFormType> MailFormTypes { get; set; }
        public DbSet<FormType> FormTypes { get; set; }
        
        // New entities for contact form with translations
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleTranslation> RoleTranslations { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<CompanyTypeTranslation> CompanyTypeTranslations { get; set; }
        public DbSet<HelpType> HelpTypes { get; set; }
        public DbSet<HelpTypeTranslation> HelpTypeTranslations { get; set; }
    }
}