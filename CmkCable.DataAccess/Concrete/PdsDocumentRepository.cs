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
                    Image = pdsDocumentDTO.Image
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
                var pdsDocumentsWithTranslations = from pd in cmkCableDbContext.PdsDocuments
                                                   join pdt in cmkCableDbContext.PdsDocumentsTranslations
                                                   on pd.Id equals pdt.PdsId
                                                   where pdt.LanguageId == languageId
                                                   select new
                                                   {
                                                       pd.Id,
                                                       pd.Image,
                                                       pd.FileContent,
                                                       Translation = new PdsDocumentTranslationDTO
                                                       {
                                                           LanguageId = pdt.LanguageId,
                                                           Name = pdt.Name
                                                       }
                                                   };

                var documentDtos = pdsDocumentsWithTranslations
                    .Select(item => new PdsDocumentDTO
                    {
                        Id = item.Id,
                        Image = item.Image,
                        FileContent = item.FileContent,
                        Translations = new List<PdsDocumentTranslationDTO> { item.Translation }
                    })
                    .ToList();

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

        public Task<PdsDocument> UpdateCategory(PdsDocument pdsDocument, List<string> translations, List<int> languageIds)
        {
            throw new NotImplementedException();
        }
    }
}
