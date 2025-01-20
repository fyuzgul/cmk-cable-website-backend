using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IHistoryItemService
    {
        List<HistoryItemDTO> GetAllHistoryItems();
        HistoryItemDTO GetHistoryItem(int id);
        List<HistoryItemDTO> GetAllHistoryItemWithSelectedLanguage(int languageId);
        HistoryItem CreateHistoryItem(CreateHistoryItemDTO historyItemDTO, List<string> titles, List<string> descriptions, List<int> languageIds);
        HistoryItem UpdateHistoryItem(HistoryItem historyItem, List<string> titles, List<string> descriptions, List<int> languageIds);
        void DeleteHistoryItem(int id);
    }
}
