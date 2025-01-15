using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IAboutUsItemRepository
    {
        List<AboutUsItem> GetAllAboutUsItemsWithLanguage(int languageId);
        AboutUsItem CreateAboutUsItem(AboutUsItem aboutUsItem);
        AboutUsItem UpdateAboutUsItem(AboutUsItem aboutUsItem); 
        void DeleteAboutUsItem(string id);

    }
}
