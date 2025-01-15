using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using DTOs.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class StructureRepository : IStructureRepository
    {
        public async Task<Structure> CreateStructure(Structure structure, List<string> translations, List<int> languageIds)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                await cmkCableDbContext.Structures.AddAsync(structure);
                await cmkCableDbContext.SaveChangesAsync();

                for (int i = 0; i < translations.Count; i++)
                {
                    var structureTranslation = new StructureTranslation
                    {
                        StructureId = structure.Id,
                        Description = translations[i],
                        LanguageId = languageIds[i]
                    };

                    await cmkCableDbContext.StructureTranslations.AddAsync(structureTranslation);
                }
                await cmkCableDbContext.SaveChangesAsync();

                return structure;
            }
        }

        public void DeleteStructure(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedStructure = cmkCableDbContext.Structures.Find(id);

                if (deletedStructure != null)
                {
                    var translations = cmkCableDbContext.StructureTranslations
                        .Where(st => st.StructureId == id)
                        .ToList();

                    var productStructures = cmkCableDbContext.ProductStructures
                        .Where(st => st.StructureId == id) .ToList();

                    cmkCableDbContext.ProductStructures.RemoveRange(productStructures);
                    cmkCableDbContext.StructureTranslations.RemoveRange(translations);

                    cmkCableDbContext.Structures.Remove(deletedStructure);

                    cmkCableDbContext.SaveChanges();
                }
            }
        }


        public List<StructureDTO> GetAllStructures()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var structures = cmkCableDbContext.Structures
                    .Select(structure => new StructureDTO
                    {
                        Id = structure.Id,
                        Name = structure.Name,
                        Structures = cmkCableDbContext.StructureTranslations
                            .Where(translation => translation.StructureId == structure.Id)
                            .Select(translation => new StructureTranslationDTO
                            {
                                LanguageId = translation.LanguageId,
                                Description = translation.Description
                            }).ToList()
                    }).ToList();

                return structures;
            }
        }


        public StructureDTO GetStructureById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var structure = cmkCableDbContext.Structures
                    .Where(s => s.Id == id)
                    .Select(s => new StructureDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Structures = cmkCableDbContext.StructureTranslations
                            .Where(t => t.StructureId == s.Id)
                            .Select(t => new StructureTranslationDTO
                            {
                                LanguageId = t.LanguageId,
                                Description = t.Description
                            }).ToList()
                    })
                    .FirstOrDefault();

                return structure;
            }
        }

        public List<StructureDTO> GetStructuresByLanguageId(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var structures = cmkCableDbContext.StructureTranslations
                    .Where(st => st.LanguageId == languageId)
                    .Select(st => new StructureDTO
                    {
                        Id = st.StructureId,
                        Structures = new List<StructureTranslationDTO>
                        {
                    new StructureTranslationDTO
                    {
                        LanguageId = st.LanguageId,
                        Description = st.Description
                    }
                        }
                    }).ToList();

                return structures;
            }
        }


        public List<Structure> GetStructuresByProductId(int prodcutId)
        {
            throw new NotImplementedException();
        }

        public Structure UpdateStructure(Structure structure, List<string> translations, List<int> languageIds)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var existingStructure = cmkCableDbContext.Structures
                                                          .FirstOrDefault(s => s.Id == structure.Id);

                if (existingStructure == null)
                {
                    throw new Exception("Structure not found.");
                }

                for (int i = 0; i < translations.Count; i++)
                {
                    var languageId = languageIds[i];
                    var translation = translations[i];

                    var existingTranslation = cmkCableDbContext.StructureTranslations
                                                               .FirstOrDefault(st => st.LanguageId == languageId && st.StructureId == structure.Id);

                    if (existingTranslation != null)
                    {
                        existingTranslation.Description = translation;
                    }
                    else
                    {
                        // Eğer mevcut çeviri yoksa, yeni bir çeviri ekle
                        cmkCableDbContext.StructureTranslations.Add(new StructureTranslation
                        {
                            LanguageId = languageId,
                            StructureId = structure.Id,
                            Description = translation
                        });
                    }
                }

                existingStructure.Name = structure.Name;
                cmkCableDbContext.SaveChanges();

                return existingStructure; 
            }
        }





    }
}