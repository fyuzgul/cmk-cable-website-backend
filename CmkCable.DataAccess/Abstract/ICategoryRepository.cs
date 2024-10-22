using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface ICategoryRepository
    {
        public List<CategoryDTO> GetAll();
        CategoryDTO GetCategoryWithAllTranslations(int categoryId);
        List<CategoryDTO> GetAllCategoriesWithLanguage(int languageId);
        CategoryDTO GetCategoryById(int id, int languageId);
        Task<Category> CreateCategory(CategoryDTO categoryDto, List<string> translations, List<int> languageIds);
        Task<Category> UpdateCategory(Category category, List<string> translations, List<int> languageIds);
        void DeleteCategory(int id);
    }
}
