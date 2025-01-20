using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class HistoryItemManager : IHistoryItemService
    {
        private IHistoryItemRepository _historyItemRepository;
        public HistoryItemManager() { _historyItemRepository = new HistoryItemRepository(); }

        public HistoryItem CreateHistoryItem(CreateHistoryItemDTO historyItemDTO, List<string> titles, List<string> descriptions, List<int> languageIds)
        {
            return _historyItemRepository.CreateHistoryItem(historyItemDTO, titles, descriptions, languageIds);
        }

        public void DeleteHistoryItem(int id)
        {
             _historyItemRepository.DeleteHistoryItem(id);
        }

        public List<HistoryItemDTO> GetAllHistoryItems()
        {
            return _historyItemRepository.GetAllHistoryItems(); 
        }

        public List<HistoryItemDTO> GetAllHistoryItemWithSelectedLanguage(int languageId)
        {
            return _historyItemRepository.GetAllHistoryItemWithSelectedLanguage(languageId);    
        }

        public HistoryItemDTO GetHistoryItem(int id)
        {
            throw new NotImplementedException();
        }

        public HistoryItem UpdateHistoryItem(HistoryItem historyItem, List<string> titles, List<string> descriptions, List<int> languageIds)
        {
            return _historyItemRepository.UpdateHistoryItem(historyItem, titles, descriptions, languageIds);
        }
    }
}
