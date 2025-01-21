using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using DTOs;
using DTOs.Translations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Concrete
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<CategoryDTO> GetAll()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                // Tüm dilleri ve çevirileri belleğe alıyoruz.
                var allLanguages = cmkCableDbContext.Languages.ToList();
                var allCategoryTranslations = cmkCableDbContext.CategoryTranslations.ToList();

                var categoriesWithTranslations = cmkCableDbContext.Categories
                    .AsEnumerable() // LINQ to Entities dışına çıkıyoruz
                    .Select(category => new CategoryDTO
                    {
                        Id = category.Id,
                        Image = category.Image,
                        Translations = allLanguages.Select(language =>
                        {
                            // Çeviriyi buluyoruz, dilde varsa alıyoruz
                            var translation = allCategoryTranslations
                                .FirstOrDefault(t => t.CategoryId == category.Id && t.LanguageId == language.Id);

                            if (translation != null)
                            {
                                return new CategoryTranslationDTO
                                {
                                    LanguageId = language.Id,
                                    Name = translation.Name
                                };
                            }

                            // Eğer dilde çeviri yoksa, 2. dildeki çeviriyi kontrol ediyoruz
                            var fallbackTranslation = allCategoryTranslations
                                .FirstOrDefault(t => t.CategoryId == category.Id && t.LanguageId == 2);

                            // 2. dilde de çeviri yoksa, "Çevirisi yok" yazıyoruz
                            return new CategoryTranslationDTO
                            {
                                LanguageId = language.Id,
                                Name = fallbackTranslation?.Name ?? "Çevirisi yok"
                            };
                        }).ToList()
                    }).ToList();

                return categoriesWithTranslations;
            }
        }


        public async Task<Category> CreateCategory(CategoryDTO categoryDto, List<string> translations, List<int> languageIds)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var category = new Category
                {
                    Image = categoryDto.Image
                };

                cmkCableDbContext.Categories.Add(category);
                await cmkCableDbContext.SaveChangesAsync();

                for (int i = 0; i < translations.Count; i++)
                {
                    var categoryTranslation = new CategoryTranslation
                    {
                        Name = translations[i],
                        CategoryId = category.Id,
                        LanguageId = languageIds[i]
                    };

                    cmkCableDbContext.CategoryTranslations.Add(categoryTranslation);
                }

                await cmkCableDbContext.SaveChangesAsync();

                return category;
            }
        }

        public void DeleteCategory(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var translations = cmkCableDbContext.CategoryTranslations
                                                    .Where(ct => ct.CategoryId == id)
                                                    .ToList();

                if (translations.Any())
                {
                    cmkCableDbContext.CategoryTranslations.RemoveRange(translations);
                }

                var deletedCategory = cmkCableDbContext.Categories.Find(id);
                if (deletedCategory != null)
                {
                    cmkCableDbContext.Categories.Remove(deletedCategory);
                    cmkCableDbContext.SaveChanges();
                }
            }
        }


        public List<CategoryDTO> GetAllCategoriesWithLanguage(int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var categoriesWithTranslations = from ni in cmkCableDbContext.Categories
                                                 join nit in cmkCableDbContext.CategoryTranslations
                                                 on ni.Id equals nit.CategoryId into translations
                                                 from translation in translations.Where(t => t.LanguageId == languageId).DefaultIfEmpty()
                                                 select new
                                                 {
                                                     ni.Id,
                                                     ni.Image,
                                                     Translation = translation != null
                                                         ? new CategoryTranslationDTO
                                                         {
                                                             LanguageId = translation.LanguageId,
                                                             Name = translation.Name
                                                         }
                                                         : null
                                                 };

                var categoryDtos = categoriesWithTranslations
                    .ToList()
                    .Select(item =>
                    {
                        CategoryTranslationDTO fallbackTranslation = null;

                        if (item.Translation == null)
                        {
                            fallbackTranslation = cmkCableDbContext.CategoryTranslations
                                .Where(t => t.CategoryId == item.Id && t.LanguageId == 2)
                                .Select(t => new CategoryTranslationDTO
                                {
                                    LanguageId = t.LanguageId,
                                    Name = t.Name ?? "Çevirisi yok"
                                })
                                .FirstOrDefault();
                        }

                        return new CategoryDTO
                        {
                            Id = item.Id,
                            Image = item.Image,
                            Translations = new List<CategoryTranslationDTO>
                            {
                        item.Translation ?? fallbackTranslation ?? new CategoryTranslationDTO { LanguageId = 2, Name = "Çevirisi yok" }
                            }
                        };
                    })
                    .ToList();

                return categoryDtos;
            }
        }


        public CategoryDTO GetCategoryById(int id, int languageId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var category = (from c in cmkCableDbContext.Categories
                                where c.Id == id
                                select new
                                {
                                    Category = c,
                                    Image = c.Image
                                }).FirstOrDefault();

                if (category == null)
                {
                    return null;
                }

                var translation = (from ct in cmkCableDbContext.CategoryTranslations
                                   where ct.CategoryId == id && ct.LanguageId == languageId
                                   select new CategoryTranslationDTO
                                   {
                                       LanguageId = ct.LanguageId,
                                       Name = ct.Name
                                   }).FirstOrDefault();

                if (translation == null)
                {
                    translation = (from ct in cmkCableDbContext.CategoryTranslations
                                   where ct.CategoryId == id && ct.LanguageId == 2
                                   select new CategoryTranslationDTO
                                   {
                                       LanguageId = ct.LanguageId,
                                       Name = ct.Name
                                   }).FirstOrDefault();
                }

                if (translation == null)
                {
                    translation = new CategoryTranslationDTO
                    {
                        LanguageId = languageId,
                        Name = "Translation not available"
                    };
                }

                return new CategoryDTO
                {
                    Id = category.Category.Id,
                    Image = category.Image,
                    Translations = new List<CategoryTranslationDTO> { translation }
                };
            }
        }

        public CategoryDTO GetCategoryWithAllTranslations(int categoryId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var categoryWithTranslations = from category in cmkCableDbContext.Categories
                                               where category.Id == categoryId
                                               select new
                                               {
                                                   Category = category,
                                                   Translations = (from translation in cmkCableDbContext.CategoryTranslations
                                                                   where translation.CategoryId == categoryId
                                                                   select new CategoryTranslationDTO
                                                                   {
                                                                       LanguageId = translation.LanguageId,
                                                                       Name = translation.Name
                                                                   }).ToList() 
                                               };

                var result = categoryWithTranslations.FirstOrDefault();

                if (result == null)
                {
                    return null;
                }

                return new CategoryDTO
                {
                    Id = result.Category.Id,
                    Image = result.Category.Image,
                    Translations = result.Translations
                };
            }
        }

        public async Task<Category> UpdateCategory(Category category, List<string> translations, List<int> languageIds)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var existingCategory = await cmkCableDbContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == category.Id);

                if (existingCategory == null)
                {
                    throw new ArgumentException("Kategori bulunamadı.");
                }


                if (category.Image != null)
                {
                    existingCategory.Image = category.Image;
                    cmkCableDbContext.Categories.Update(existingCategory);
                    cmkCableDbContext.SaveChanges();
                }

                for (int i = 0; i < translations.Count; i++)
                {
                    var translation = await cmkCableDbContext.CategoryTranslations
                        .FirstOrDefaultAsync(ct => ct.CategoryId == existingCategory.Id && ct.LanguageId == languageIds[i]);

                    if (translation != null)
                    {
                        translation.Name = translations[i];
                    }
                    else
                    {
                        var newTranslation = new CategoryTranslation
                        {
                            Name = translations[i],
                            CategoryId = existingCategory.Id,
                            LanguageId = languageIds[i]
                        };
                        cmkCableDbContext.CategoryTranslations.Add(newTranslation);
                    }
                }

                var existingTranslations = await cmkCableDbContext.CategoryTranslations
                    .Where(ct => ct.CategoryId == existingCategory.Id)
                    .ToListAsync();

                var existingLanguageIds = existingTranslations.Select(ct => ct.LanguageId).ToList();
                foreach (var languageId in existingLanguageIds)
                {
                    if (!languageIds.Contains(languageId))
                    {
                        var translationToRemove = existingTranslations.First(ct => ct.LanguageId == languageId);
                        cmkCableDbContext.CategoryTranslations.Remove(translationToRemove);
                    }
                }

                await cmkCableDbContext.SaveChangesAsync();

                return existingCategory;
            }
        }


    }
}
