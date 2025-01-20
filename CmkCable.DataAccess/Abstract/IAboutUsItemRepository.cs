using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IAboutUsItemRepository
    {
        List<AboutUsItem> GetAllAboutUsItemsWithLanguage(int languageId);
        List<AboutUsItem> GetAllAboutUsItems(); 
        AboutUsItem CreateAboutUsItem(AboutUsItem aboutUsItem);
        AboutUsItem UpdateAboutUsItem(int id, AboutUsItem aboutUsItem); 
        void DeleteAboutUsItem(int id);

    }
}
