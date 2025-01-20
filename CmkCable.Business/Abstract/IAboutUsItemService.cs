using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IAboutUsItemService
    {
        List<AboutUsItem> GetAllAboutUsItemsWithLanguage(int languageId);
        List<AboutUsItem> GetAllAboutUsItems();
        AboutUsItem CreateAboutUsItem(AboutUsItem aboutUsItem);
        AboutUsItem UpdateAboutUsItem(int id, AboutUsItem aboutUsItem);
        void DeleteAboutUsItem(int id);
    }
}
