using CmkCable.Entities;
using DTOs;
using DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IHomePageTextService
    {
        HomePageTextDTO GetHomePageTextByName(string name, int languageId);
        List<HomePageTextDTO> GetHomeAllPageTexts(int languageId);
        List<HomePageTextDTO> GetHomePageTextsWithAllTranslations();
        List<HomePageTextUpdateDTO> UpdateHomeText(List<HomePageTextUpdateDTO> homePageTextUpdateDTOs);

    }
}
