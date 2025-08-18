using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IHomePageTextRepository
    {
        HomePageTextDTO GetHomePageTextByName(string Name, int languageId);
        HomePageTextDTO GetHomePageTextById(int id, int languageId);
        List<HomePageTextDTO> GetHomeAllPageTexts(int languageId);
        List<HomePageTextDTO> GetHomePageTextsWithAllTranslations();
        List<HomePageTextUpdateDTO> UpdateHomeText(List<HomePageTextUpdateDTO> homePageTextUpdateDTOs);
        HomePageTextDTO CreateHomePageText(CreateHomePageTextWithTranslationsDTO createDto);
        bool DeleteHomePageText(int id);
    }
}
