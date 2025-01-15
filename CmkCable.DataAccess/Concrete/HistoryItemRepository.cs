using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class HistoryItemRepository : IHistoryItemRepository
    {
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
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var historyItemsWithSelectedLanguage = from hi in cmkCableDbContext.HistoryItems
                                                       join hit in cmkCableDbContext.HistoryItemTranslations
                                                       on hi.Id equals hit.HistoryItemId
                                                       where hit.LanguageId == languageId
                                                       select new
                                                       {
                                                           hi.Id,
                                                           hi.Image,
                                                           hi.Year,
                                                           Translation = new HistoryItemTranslationDTO
                                                           {
                                                               LanguageId = hit.LanguageId,
                                                               Title = hit.Title,
                                                               Description = hit.Description
                                                           }

                                                       };
                var historyItemDTO = historyItemsWithSelectedLanguage
                    .Select(item => new HistoryItemDTO
                    {
                        Id = item.Id,
                        Year = item.Year,
                        Image = item.Image,
                        Translations = new List<HistoryItemTranslationDTO> { item.Translation }
                    })
                    .ToList();

                return historyItemDTO;
            }
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
