using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IHistoryItemService
    {
        List<HistoryItem> GetAllHistoryItems();
        HistoryItem GetHistoryItem(int id);
        HistoryItem CreateHistoryItem(HistoryItem historyItem);
        HistoryItem UpdateHistoryItem(HistoryItem historyItem);
        void DeleteHistoryItem(int id);
    }
}
