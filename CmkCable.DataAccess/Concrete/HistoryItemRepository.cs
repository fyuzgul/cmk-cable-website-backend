using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class HistoryItemRepository : IHistoryItemRepository
    {
        public HistoryItem CreateHistoryItem(CreateHistoryItemDTO historyItemDTO, List<string> titles, List<string>descriptions, List<int> languageIds )
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var historyItem = new HistoryItem
                {
                    Image = historyItemDTO.Image,
                    Year = historyItemDTO.Year
                };  
                cmkCableDbContext.HistoryItems.Add(historyItem);
                cmkCableDbContext.SaveChanges();

                for (int i = 0; i < languageIds.Count; i++)
                {
                    cmkCableDbContext.HistoryItemTranslations.Add(new HistoryItemTranslation
                    {
                        HistoryItemId = historyItem.Id,
                        LanguageId = languageIds[i],
                        Title = titles[i],
                        Description = descriptions[i]
                    });
                }

                cmkCableDbContext.SaveChanges();
                return historyItem;
            }

        }

        public void DeleteHistoryItem(int id)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                var historyItem = cmkCableDbContext.HistoryItems.Find(id);
                cmkCableDbContext.HistoryItems.Remove(historyItem);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<HistoryItemDTO> GetAllHistoryItems()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                // Desteklenen dillerin listesi
                var supportedLanguages = cmkCableDbContext.Languages.Select(l => l.Id).ToList();

                var historyItems = from hi in cmkCableDbContext.HistoryItems
                                   join hit in cmkCableDbContext.HistoryItemTranslations
                                   on hi.Id equals hit.HistoryItemId
                                   select new
                                   {
                                       hi.Id,
                                       hi.Image,
                                       hi.Year,
                                       hit.LanguageId,
                                       hit.Title,
                                       hit.Description
                                   };

                var historyItemDTO = historyItems
                    .ToList()
                    .GroupBy(item => item.Id)
                    .Select(group =>
                    {
                        var translations = supportedLanguages.Select(languageId =>
                        {
                            var translation = group.FirstOrDefault(x => x.LanguageId == languageId);
                            if (translation != null)
                            {
                                return new HistoryItemTranslationDTO
                                {
                                    LanguageId = translation.LanguageId,
                                    Title = translation.Title ?? "Çevirisi yok",
                                    Description = translation.Description ?? "Çevirisi yok"
                                };
                            }
                            else
                            {
                                // Eğer dilde veri yoksa, 2. dildeki veriyi kullanıyoruz.
                                var fallbackTranslation = group.FirstOrDefault(x => x.LanguageId == 2);
                                return new HistoryItemTranslationDTO
                                {
                                    LanguageId = languageId,
                                    Title = fallbackTranslation?.Title ?? "Çevirisi yok",
                                    Description = fallbackTranslation?.Description ?? "Çevirisi yok"
                                };
                            }
                        }).ToList();

                        return new HistoryItemDTO
                        {
                            Id = group.Key,
                            Year = group.FirstOrDefault()?.Year,
                            Image = group.FirstOrDefault()?.Image,
                            Translations = translations
                        };
                    })
                    .ToList();

                return historyItemDTO;
            }
        }

        public List<HistoryItemDTO> GetAllHistoryItemWithSelectedLanguage(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var historyItemsWithSelectedLanguage = from hi in cmkCableDbContext.HistoryItems
                                                       join hit in cmkCableDbContext.HistoryItemTranslations
                                                       on hi.Id equals hit.HistoryItemId
                                                       where hit.LanguageId == languageId || hit.LanguageId == 2
                                                       select new
                                                       {
                                                           hi.Id,
                                                           hi.Image,
                                                           hi.Year,
                                                           hit.LanguageId,
                                                           hit.Title,
                                                           hit.Description
                                                       };

                var historyItemDTO = historyItemsWithSelectedLanguage
                    .ToList()  
                    .GroupBy(item => item.Id) 
                    .Select(group =>
                    {
                        var selectedTranslation = group.FirstOrDefault(x => x.LanguageId == languageId) ??
                                                  group.FirstOrDefault(x => x.LanguageId == 2);

                        return new HistoryItemDTO
                        {
                            Id = group.Key,
                            Year = group.FirstOrDefault()?.Year,
                            Image = group.FirstOrDefault()?.Image,
                            Translations = selectedTranslation != null
                                ? new List<HistoryItemTranslationDTO>
                                {
                            new HistoryItemTranslationDTO
                            {
                                LanguageId = selectedTranslation.LanguageId,
                                Title = selectedTranslation.Title ?? "Çevirisi yok",
                                Description = selectedTranslation.Description ?? "Çevirisi yok"
                            }
                                }
                                : new List<HistoryItemTranslationDTO>
                                {
                            new HistoryItemTranslationDTO
                            {
                                LanguageId = 0,
                                Title = "Çevirisi yok",
                                Description = "Çevirisi yok"
                            }
                                }
                        };
                    })
                    .ToList();

                return historyItemDTO;
            }
        }

        public HistoryItemDTO GetHistoryItem(int id)
        {
            throw new NotImplementedException();
        }

        public HistoryItem UpdateHistoryItem(HistoryItem historyItem, List<string> titles, List<string> descriptions, List<int> languageIds)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                var historyItemToUpdate = cmkCableDbContext.HistoryItems.Find(historyItem.Id);
                historyItemToUpdate.Image = historyItem.Image;
                historyItemToUpdate.Year = historyItem.Year;
                cmkCableDbContext.SaveChanges();

                for (int i = 0; i < languageIds.Count; i++)
                {
                    var translation = cmkCableDbContext.HistoryItemTranslations.FirstOrDefault(x => x.HistoryItemId == historyItem.Id && x.LanguageId == languageIds[i]);
                    if (translation != null)
                    {
                        translation.Title = titles[i];
                        translation.Description = descriptions[i];
                    }
                    else
                    {
                        cmkCableDbContext.HistoryItemTranslations.Add(new HistoryItemTranslation
                        {
                            HistoryItemId = historyItem.Id,
                            LanguageId = languageIds[i],
                            Title = titles[i],
                            Description = descriptions[i]
                        });
                    }
                }

                cmkCableDbContext.SaveChanges();
                return historyItem;
            }
        }
    }
}
