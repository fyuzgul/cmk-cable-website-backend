using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using DTOs.Translations;
using DTOs.UpdateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CmkCable.DataAccess.Concrete
{
    public class HomePageTextRepository : IHomePageTextRepository
    {
        public List<HomePageTextDTO> GetHomeAllPageTexts(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var texts = cmkCableDbContext.HomePageTexts
                    .Select(text => new HomePageTextDTO
                    {
                        Id = text.Id,
                        Name = text.Name,
                        Values = cmkCableDbContext.HomePageTextTranslations
                            .Where(t => t.TextId == text.Id && t.LanguageId == languageId)
                            .Select(t => new HomePageTextTranslationDTO
                            {
                                LanguageId = t.LanguageId,
                                Value = t.Value ?? "Çevirisi yok"
                            })
                            .ToList() // Sorguyu burada çalıştırıyoruz
                    })
                    .ToList(); // Tüm listeyi burada çekiyoruz

                // Alternatif dil çevirilerini kontrol et
                foreach (var text in texts)
                {
                    if (text.Values == null || !text.Values.Any())
                    {
                        var alternativeTranslation = cmkCableDbContext.HomePageTextTranslations
                            .Where(alt => alt.TextId == text.Id && alt.LanguageId == 2) // Alternatif dil ID'si
                            .Select(alt => new HomePageTextTranslationDTO
                            {
                                LanguageId = alt.LanguageId,
                                Value = alt.Value ?? "Çevirisi yok"
                            })
                            .FirstOrDefault();

                        if (alternativeTranslation != null)
                        {
                            text.Values.Add(alternativeTranslation);
                        }
                        else
                        {
                            text.Values.Add(new HomePageTextTranslationDTO
                            {
                                LanguageId = 2,
                                Value = "Çevirisi yok"
                            });
                        }
                    }
                }

                return texts;
            }
        }



        public HomePageTextDTO GetHomePageTextByName(string name, int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                throw new NotImplementedException();
            }
        }

        public List<HomePageTextDTO> GetHomePageTextsWithAllTranslations()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var texts = cmkCableDbContext.HomePageTexts
                    .Select(text => new HomePageTextDTO
                    {
                        Id = text.Id,
                        Name = text.Name,
                        Values = cmkCableDbContext.HomePageTextTranslations
                            .Where(t => text.Id == t.TextId)
                            .Select(t => new HomePageTextTranslationDTO
                            {
                                LanguageId = t.LanguageId,
                                Value = t.Value
                            })
                            .ToList()
                    })
                    .ToList();

                return texts;
            }
        }

        public List<HomePageTextUpdateDTO> UpdateHomeText(List<HomePageTextUpdateDTO> homePageTextUpdateDTOs)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                foreach (var updateDto in homePageTextUpdateDTOs)
                {
                    var translation = cmkCableDbContext.HomePageTextTranslations
                        .FirstOrDefault(t => t.TextId == updateDto.Id && t.LanguageId == updateDto.LanguageId);

                    if (translation == null)
                    {
                        translation = new HomePageTextTranslation
                        {
                            TextId = updateDto.Id,
                            LanguageId = updateDto.LanguageId,
                            Value = updateDto.Value
                        };
                        cmkCableDbContext.HomePageTextTranslations.Add(translation);
                    }
                    else
                    {
                        translation.Value = updateDto.Value;
                    }
                }

                cmkCableDbContext.SaveChanges();
            }

            return homePageTextUpdateDTOs;
        }


    }
}
