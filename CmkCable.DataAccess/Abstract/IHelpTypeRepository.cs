using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IHelpTypeRepository
    {
        HelpType CreateHelpType(HelpType helpType);
        HelpType CreateHelpTypeWithTranslations(HelpType helpType, List<HelpTypeTranslation> translations);
        HelpType UpdateHelpType(HelpType helpType);
        void DeleteHelpType(int id);
        HelpType GetHelpTypeById(int id);
        List<HelpType> GetAllHelpTypes();
        List<HelpType> GetActiveHelpTypes();
    }
}

