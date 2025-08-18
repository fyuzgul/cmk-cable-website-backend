using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class HelpTypeManager : IHelpTypeService
    {
        private IHelpTypeRepository _helpTypeRepository;

        public HelpTypeManager()
        {
            _helpTypeRepository = new HelpTypeRepository();
        }

        public HelpType CreateHelpType(HelpType helpType)
        {
            return _helpTypeRepository.CreateHelpType(helpType);
        }

        public HelpType CreateHelpTypeWithTranslations(HelpType helpType, List<HelpTypeTranslation> translations)
        {
            return _helpTypeRepository.CreateHelpTypeWithTranslations(helpType, translations);
        }

        public void DeleteHelpType(int id)
        {
            _helpTypeRepository.DeleteHelpType(id);
        }

        public List<HelpType> GetAllHelpTypes()
        {
            return _helpTypeRepository.GetAllHelpTypes();
        }

        public List<HelpType> GetActiveHelpTypes()
        {
            return _helpTypeRepository.GetActiveHelpTypes();
        }

        public HelpType GetHelpTypeById(int id)
        {
            return _helpTypeRepository.GetHelpTypeById(id);
        }

        public HelpType UpdateHelpType(HelpType helpType)
        {
            return _helpTypeRepository.UpdateHelpType(helpType);
        }
    }
}

