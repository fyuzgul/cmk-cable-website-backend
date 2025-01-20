using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class AboutUsItemManager : IAboutUsItemService
    {
        private IAboutUsItemRepository _aboutUsItemRepository;

        public AboutUsItemManager()
        {
            _aboutUsItemRepository = new AboutUsItemRepository();
        }
        public AboutUsItem CreateAboutUsItem(AboutUsItem aboutUsItem)
        {
            return _aboutUsItemRepository.CreateAboutUsItem(aboutUsItem);   
        }

        public void DeleteAboutUsItem(int id)
        {
            _aboutUsItemRepository.DeleteAboutUsItem(id);
        }

        public List<AboutUsItem> GetAllAboutUsItems()
        {
            return _aboutUsItemRepository.GetAllAboutUsItems();
        }

        public List<AboutUsItem> GetAllAboutUsItemsWithLanguage(int languageId)
        {
            return _aboutUsItemRepository.GetAllAboutUsItemsWithLanguage(languageId);
        }

        public AboutUsItem UpdateAboutUsItem(int id, AboutUsItem aboutUsItem)
        {
            return _aboutUsItemRepository.UpdateAboutUsItem(id, aboutUsItem);
        }
    }
}
