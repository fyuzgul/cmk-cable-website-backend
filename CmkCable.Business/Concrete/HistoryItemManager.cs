using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
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
            throw new NotImplementedException();
        }

        public void DeleteHistoryItem(int id)
        {
            throw new NotImplementedException();
        }

        public List<HistoryItemDTO> GetAllHistoryItems()
        {
            throw new NotImplementedException();
        }

        public List<HistoryItemDTO> GetAllHistoryItemWithSelectedLanguage(int languageId)
        {
            return _historyItemRepository.GetAllHistoryItemWithSelectedLanguage(languageId);    
        }

        public HistoryItemDTO GetHistoryItem(int id)
        {
            throw new NotImplementedException();
        }

        public HistoryItem UpdateHistoryItem(HistoryItem historyItem)
        {
            throw new NotImplementedException();
        }
    }
}
