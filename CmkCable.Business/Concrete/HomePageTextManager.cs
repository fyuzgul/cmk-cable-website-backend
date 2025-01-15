using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class HomePageTextManager : IHomePageTextService
    {
        private IHomePageTextRepository _homePageTextRepository;
        public HomePageTextManager() { _homePageTextRepository = new HomePageTextRepository(); }

        public List<HomePageTextDTO> GetHomeAllPageTexts(int languageId)
        {
            return _homePageTextRepository.GetHomeAllPageTexts(languageId); 
        }

        public HomePageTextDTO GetHomePageTextByName(string name, int languageId)
        {
           return _homePageTextRepository.GetHomePageTextByName(name, languageId);    
        }

        public List<HomePageTextDTO> GetHomePageTextsWithAllTranslations()
        {
            return _homePageTextRepository.GetHomePageTextsWithAllTranslations();
        }
        public List<HomePageTextUpdateDTO> UpdateHomeText(List<HomePageTextUpdateDTO> homePageTextUpdateDTOs)
        {
           return _homePageTextRepository.UpdateHomeText(homePageTextUpdateDTOs);
        }   
    }
}
