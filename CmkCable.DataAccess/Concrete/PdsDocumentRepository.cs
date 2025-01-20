using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using System.Linq;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTOs.Translations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CmkCable.DataAccess.Concrete
{
    public class PdsDocumentRepository : IPdsDocumentRepository
    {
        public async Task<PdsDocument> CreatePdsDocument(PdsDocumentDTO pdsDocumentDTO, List<string> translations, List<int> languageIds)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var pdsDocument = new PdsDocument
                {
                    FileContent = pdsDocumentDTO.FileContent,
                    Image = pdsDocumentDTO.Image,
                };

                cmkCableDbContext.PdsDocuments.Add(pdsDocument);
                await cmkCableDbContext.SaveChangesAsync();

                for (int i = 0; i < translations.Count; i++)
                {
                    var pdsDocumentTranslation = new PdsDocumentTranslation
                    {
                        Name = translations[i],
                        PdsId = pdsDocument.Id,
                        LanguageId = languageIds[i]
                    };

                    cmkCableDbContext.PdsDocumentsTranslations.Add(pdsDocumentTranslation);
                }

                await cmkCableDbContext.SaveChangesAsync();
                return pdsDocument; 

            }
        }

        public void DeletePdsDocument(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var translations = cmkCableDbContext.PdsDocumentsTranslations
                                                    .Where(ct => ct.PdsId == id)
                                                    .ToList();

                if (translations.Any())
                {
                    cmkCableDbContext.PdsDocumentsTranslations.RemoveRange(translations);
                }

                var deletedDocument = cmkCableDbContext.PdsDocuments.Find(id);
                if (deletedDocument != null)
                {
                    cmkCableDbContext.PdsDocuments.Remove(deletedDocument);
                    cmkCableDbContext.SaveChanges();
                }
            }
        }

        public List<PdsDocumentDTO> GetAll()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var pdfDocumentsWithTranslations = from pdsDocument in cmkCableDbContext.PdsDocuments
                                                   select new PdsDocumentDTO
                                                   {
                                                       Id = pdsDocument.Id,
                                                       Image = pdsDocument.Image,
                                                       FileContent = pdsDocument.FileContent,
                                                       Translations = (from translation in cmkCableDbContext.PdsDocumentsTranslations
                                                                       where translation.PdsId == pdsDocument.Id
                                                                       select new PdsDocumentTranslationDTO
                                                                       {
                                                                           LanguageId = translation.LanguageId,
                                                                           Name = translation.Name
                                                                       }).ToList()
                                                   };

                return pdfDocumentsWithTranslations.ToList();
            }
        }

        public List<PdsDocumentDTO> GetAllPdsDocumentsWithLanguage(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                // 1. Adım: PdsDocuments tablosunu alıyoruz
                var pdsDocuments = cmkCableDbContext.PdsDocuments.ToList();

                // 2. Adım: Seçilen dildeki çevirileri alıyoruz
                var translationsForSelectedLanguage = cmkCableDbContext.PdsDocumentsTranslations
                    .Where(pdt => pdt.LanguageId == languageId)
                    .ToList();

                // 3. Adım: Seçilen dilde çeviri olmayan belgeleri ikinci dilde (örneğin, 2 numaralı dil) kontrol ediyoruz
                var translationsForSecondLanguage = cmkCableDbContext.PdsDocumentsTranslations
                    .Where(pdt => pdt.LanguageId == 2) // 2 numaralı dil (örneğin Türkçe)
                    .ToList();

                // 4. Adım: Şimdi her bir PdsDocument için dil çevirisini bulalım
                var documentDtos = new List<PdsDocumentDTO>();

                foreach (var pd in pdsDocuments)
                {
                    // Seçilen dildeki çeviriyi bulalım
                    var selectedLanguageTranslation = translationsForSelectedLanguage
                        .FirstOrDefault(pdt => pdt.PdsId == pd.Id);

                    // Eğer seçilen dilde çeviri yoksa, ikinci dildeki çeviriyi bulalım
                    var secondLanguageTranslation = selectedLanguageTranslation == null
                        ? translationsForSecondLanguage.FirstOrDefault(pdt => pdt.PdsId == pd.Id)
                        : null;

                    // Eğer her iki dilde de çeviri yoksa, "Çeviri Yok" mesajını ekleyelim
                    var translations = new List<PdsDocumentTranslationDTO>();

                    if (selectedLanguageTranslation != null)
                    {
                        translations.Add(new PdsDocumentTranslationDTO
                        {
                            LanguageId = selectedLanguageTranslation.LanguageId,
                            Name = selectedLanguageTranslation.Name
                        });
                    }
                    else if (secondLanguageTranslation != null)
                    {
                        translations.Add(new PdsDocumentTranslationDTO
                        {
                            LanguageId = secondLanguageTranslation.LanguageId,
                            Name = secondLanguageTranslation.Name
                        });
                    }
                    else
                    {
                        translations.Add(new PdsDocumentTranslationDTO
                        {
                            LanguageId = 0, // Özel bir ID ya da 0
                            Name = "Çeviri Yok"
                        });
                    }

                    // Her belge için DTO oluşturuyoruz
                    documentDtos.Add(new PdsDocumentDTO
                    {
                        Id = pd.Id,
                        Image = pd.Image,
                        FileContent = pd.FileContent,
                        Translations = translations
                    });
                }

                return documentDtos;
            }
        }


        public PdsDocumentDTO PdsDocumentWithAllTranslations(int pdsId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var documentWithTranslations = from pdsDocument in cmkCableDbContext.PdsDocuments
                                               where pdsDocument.Id == pdsId
                                               select new
                                               {
                                                   PdsDocument = pdsDocument,
                                                   Translations = (from translation in cmkCableDbContext.PdsDocumentsTranslations
                                                                   where translation.PdsId == pdsId
                                                                   select new PdsDocumentTranslationDTO
                                                                   {
                                                                       LanguageId = translation.LanguageId,
                                                                       Name = translation.Name
                                                                   }).ToList()
                                               };

                var result = documentWithTranslations.FirstOrDefault();

                if (result == null)
                {
                    return null;
                }

                return new PdsDocumentDTO
                {
                    Id = result.PdsDocument.Id,
                    Image = result.PdsDocument.Image,
                    FileContent= result.PdsDocument.FileContent,
                    Translations = result.Translations
                };
            }
        }

        public async Task<PdsDocument> UpdatePdsDocument(PdsDocument pdsDocument, List<string> translations, List<int> languageIds)
        {
            using (var context = new CmkCableDbContext())
            {
                var existingDocument = await context.PdsDocuments.FindAsync(pdsDocument.Id);
                if (existingDocument == null)
                {
                    throw new Exception("Document not found");
                }

                existingDocument.Image = pdsDocument.Image;
                existingDocument.FileContent = pdsDocument.FileContent;

                var existingTranslations = context.PdsDocumentsTranslations
                                                  .Where(t => t.PdsId == pdsDocument.Id)
                                                  .ToList();

                for (int i = 0; i < translations.Count; i++)
                {
                    var existingTranslation = existingTranslations
                                              .FirstOrDefault(t => t.LanguageId == languageIds[i]);

                    if (existingTranslation != null)
                    {
                        existingTranslation.Name = translations[i];
                    }
                    else
                    {
                        var pdsDocumentTranslation = new PdsDocumentTranslation
                        {
                            Name = translations[i],
                            PdsId = pdsDocument.Id,
                            LanguageId = languageIds[i]
                        };
                        context.PdsDocumentsTranslations.Add(pdsDocumentTranslation);
                    }
                }

                await context.SaveChangesAsync();

                return existingDocument;
            }
        }


    }
}
