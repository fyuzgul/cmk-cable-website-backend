using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IHistoryItemRepository
    {
        List<HistoryItemDTO> GetAllHistoryItems();
        HistoryItemDTO GetHistoryItem(int id);
        List<HistoryItemDTO> GetAllHistoryItemWithSelectedLanguage(int languageId);
        HistoryItem CreateHistoryItem(HistoryItem historyItem); 
        HistoryItem UpdateHistoryItem(HistoryItem historyItem);
        void DeleteHistoryItem(int id);  
    }
}
