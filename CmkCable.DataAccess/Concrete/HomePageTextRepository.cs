using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
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
                var text = cmkCableDbContext.HomePageTexts.FirstOrDefault(t => t.Name == name);
                if (text == null)
                    return null;

                var translation = cmkCableDbContext.HomePageTextTranslations
                    .Where(t => t.TextId == text.Id && t.LanguageId == languageId)
                    .Select(t => new HomePageTextTranslationDTO
                    {
                        LanguageId = t.LanguageId,
                        Value = t.Value ?? "Çevirisi yok"
                    })
                    .FirstOrDefault();

                if (translation == null)
                {
                    // Try alternative language
                    translation = cmkCableDbContext.HomePageTextTranslations
                        .Where(t => t.TextId == text.Id && t.LanguageId == 2)
                        .Select(t => new HomePageTextTranslationDTO
                        {
                            LanguageId = t.LanguageId,
                            Value = t.Value ?? "Çevirisi yok"
                        })
                        .FirstOrDefault();

                    if (translation == null)
                    {
                        translation = new HomePageTextTranslationDTO
                        {
                            LanguageId = 2,
                            Value = "Çevirisi yok"
                        };
                    }
                }

                return new HomePageTextDTO
                {
                    Id = text.Id,
                    Name = text.Name,
                    Values = new List<HomePageTextTranslationDTO> { translation }
                };
            }
        }

        public HomePageTextDTO GetHomePageTextById(int id, int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var text = cmkCableDbContext.HomePageTexts.FirstOrDefault(t => t.Id == id);
                if (text == null)
                    return null;

                var translation = cmkCableDbContext.HomePageTextTranslations
                    .Where(t => t.TextId == id && t.LanguageId == languageId)
                    .Select(t => new HomePageTextTranslationDTO
                    {
                        LanguageId = t.LanguageId,
                        Value = t.Value ?? "Çevirisi yok"
                    })
                    .FirstOrDefault();

                if (translation == null)
                {
                    // Try alternative language
                    translation = cmkCableDbContext.HomePageTextTranslations
                        .Where(t => t.TextId == id && t.LanguageId == 2)
                        .Select(t => new HomePageTextTranslationDTO
                        {
                            LanguageId = t.LanguageId,
                            Value = t.Value ?? "Çevirisi yok"
                        })
                        .FirstOrDefault();

                    if (translation == null)
                    {
                        translation = new HomePageTextTranslationDTO
                        {
                            LanguageId = 2,
                            Value = "Çevirisi yok"
                        };
                    }
                }

                return new HomePageTextDTO
                {
                    Id = text.Id,
                    Name = text.Name,
                    Values = new List<HomePageTextTranslationDTO> { translation }
                };
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

        public HomePageTextDTO CreateHomePageText(CreateHomePageTextWithTranslationsDTO createDto)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                // Create the main HomePageText entity
                var homePageText = new HomePageText
                {
                    Name = createDto.Name
                };

                cmkCableDbContext.HomePageTexts.Add(homePageText);
                cmkCableDbContext.SaveChanges(); // Save to get the ID

                // Create translations
                if (createDto.Translations != null && createDto.Translations.Any())
                {
                    foreach (var translationDto in createDto.Translations)
                    {
                        var translation = new HomePageTextTranslation
                        {
                            TextId = homePageText.Id,
                            LanguageId = translationDto.LanguageId,
                            Value = translationDto.Value
                        };

                        cmkCableDbContext.HomePageTextTranslations.Add(translation);
                    }
                    cmkCableDbContext.SaveChanges();
                }

                // Return the created DTO
                return new HomePageTextDTO
                {
                    Id = homePageText.Id,
                    Name = homePageText.Name,
                    Values = createDto.Translations ?? new List<HomePageTextTranslationDTO>()
                };
            }
        }

        public bool DeleteHomePageText(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var homePageText = cmkCableDbContext.HomePageTexts.FirstOrDefault(ht => ht.Id == id);
                if (homePageText == null)
                    return false;

                // Delete related translations first
                var translations = cmkCableDbContext.HomePageTextTranslations.Where(t => t.TextId == id);
                cmkCableDbContext.HomePageTextTranslations.RemoveRange(translations);

                // Delete the main entity
                cmkCableDbContext.HomePageTexts.Remove(homePageText);
                cmkCableDbContext.SaveChanges();

                return true;
            }
        }
    }
}
