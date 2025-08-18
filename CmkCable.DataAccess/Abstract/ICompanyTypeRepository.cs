using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface ICompanyTypeRepository
    {
        CompanyType CreateCompanyType(CompanyType companyType);
        CompanyType CreateCompanyTypeWithTranslations(CompanyType companyType, List<CompanyTypeTranslation> translations);
        CompanyType UpdateCompanyType(CompanyType companyType);
        void DeleteCompanyType(int id);
        CompanyType GetCompanyTypeById(int id);
        List<CompanyType> GetAllCompanyTypes();
        List<CompanyType> GetActiveCompanyTypes();
    }
}

