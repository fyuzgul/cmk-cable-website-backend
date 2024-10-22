using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class HistoryItemManager : IHistoryItemService
    {
        private IHistoryItemRepository _historyItemRepository;
        public HistoryItemManager() { _historyItemRepository = new HistoryItemRepository(); }
        public HistoryItem CreateHistoryItem(HistoryItem historyItem)
        {
            return _historyItemRepository.CreateHistoryItem(historyItem);
        }

        public void DeleteHistoryItem(int id)
        {
            _historyItemRepository.DeleteHistoryItem(id);
        }

        public List<HistoryItem> GetAllHistoryItems()
        {
            return _historyItemRepository.GetAllHistoryItems();
        }

        public HistoryItem GetHistoryItem(int id)
        {
           return _historyItemRepository.GetHistoryItem(id);
        }

        public HistoryItem UpdateHistoryItem(HistoryItem historyItem)
        {
            return _historyItemRepository.UpdateHistoryItem(historyItem);
        }
    }
}
