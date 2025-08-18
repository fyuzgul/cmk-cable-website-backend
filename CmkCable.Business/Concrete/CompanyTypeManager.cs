using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class CompanyTypeManager : ICompanyTypeService
    {
        private ICompanyTypeRepository _companyTypeRepository;

        public CompanyTypeManager()
        {
            _companyTypeRepository = new CompanyTypeRepository();
        }

        public CompanyType CreateCompanyType(CompanyType companyType)
        {
            return _companyTypeRepository.CreateCompanyType(companyType);
        }

        public CompanyType CreateCompanyTypeWithTranslations(CompanyType companyType, List<CompanyTypeTranslation> translations)
        {
            return _companyTypeRepository.CreateCompanyTypeWithTranslations(companyType, translations);
        }

        public void DeleteCompanyType(int id)
        {
            _companyTypeRepository.DeleteCompanyType(id);
        }

        public List<CompanyType> GetAllCompanyTypes()
        {
            return _companyTypeRepository.GetAllCompanyTypes();
        }

        public List<CompanyType> GetActiveCompanyTypes()
        {
            return _companyTypeRepository.GetActiveCompanyTypes();
        }

        public CompanyType GetCompanyTypeById(int id)
        {
            return _companyTypeRepository.GetCompanyTypeById(id);
        }

        public CompanyType UpdateCompanyType(CompanyType companyType)
        {
            return _companyTypeRepository.UpdateCompanyType(companyType);
        }
    }
}

