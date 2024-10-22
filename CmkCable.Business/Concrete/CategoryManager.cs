using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
//iş kuralları için
namespace CmkCable.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        public CategoryManager() { 
           _categoryRepository = new CategoryRepository();
        }

        public void DeleteCategory(int id)
        {
            _categoryRepository.DeleteCategory(id);
        }

        

        public List<CategoryDTO> GetAllCategoriesWithLanguage(int languageId)
        {
            return _categoryRepository.GetAllCategoriesWithLanguage(languageId);
        }

        public CategoryDTO GetCategoryById(int id, int languageId)
        {
            return _categoryRepository.GetCategoryById(id, languageId);
        }

        Task<Category> ICategoryService.UpdateCategory(Category category, List<string> translations, List<int> languageIds)
        {
            return _categoryRepository.UpdateCategory(category,  translations,languageIds);
        }

        Task<Category> ICategoryService.CreateCategory(CategoryDTO categoryDto, List<string> translations, List<int> languageIds)
        {
            return _categoryRepository.CreateCategory(categoryDto, translations, languageIds);
        }


        public CategoryDTO GetCategoryWithAllTranslations(int categoryId)
        {
            return _categoryRepository.GetCategoryWithAllTranslations(categoryId);
        }

        public List<CategoryDTO> GetAll()
        {
            return _categoryRepository.GetAll();
        }
    }
}
