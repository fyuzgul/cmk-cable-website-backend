using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
using DTOs.UpdateDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        private CloudinaryManager _cloudinaryManager;

        public CategoriesController()
        {
            _categoryService = new CategoryManager();
            _cloudinaryManager = new CloudinaryManager();
        }

        [HttpGet]
        public List<CategoryDTO> GetAll()
        {
            return _categoryService.GetAll();
        }

        [HttpGet("byLanguage/{languageId}")]
        public IActionResult GetAllCategoriesWithLanguage(int languageId)
        {
            var categories = _categoryService.GetAllCategoriesWithLanguage(languageId);
            return Ok(categories);
        }

        [HttpGet("{id}/{languageId}")]
        public IActionResult Get(int id, int languageId)
        {
            var category = _categoryService.GetCategoryById(id, languageId);

            if (category == null)
            {
                return NotFound($"Kategori bulunamadı. ID: {id}, Dil ID: {languageId}");
            }

            return Ok(category);
        }
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryDTO categoryDto, [FromForm] List<string> translations, [FromForm] List<int> languageIds)
        {
            string imageUrl = null;
            if (categoryDto.Image != null && categoryDto.Image.Length > 0)
            {
                imageUrl = await _cloudinaryManager.UploadImage(categoryDto.Image, "category-images");
            }
            var category = new CategoryDTO
            {
                Image = imageUrl
            };
            var createdCategory = await _categoryService.CreateCategory(category, translations, languageIds);

            return CreatedAtAction(nameof(createdCategory), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(
     [FromForm] UpdateCategoryDTO category,
     [FromForm] List<string> translations,
     [FromForm] List<int> languageIds)
        {
            string imageUrl = null;
            var _category = _categoryService.GetCategoryById(category.Id, 1);
            if (_category == null || _category.Id <= 0)
                return BadRequest("Invalid product ID.");

            if (translations == null || languageIds == null || translations.Count != languageIds.Count)
            {
                return BadRequest("Translations and language IDs must have the same length.");
            }
            if (category.Image != null)
            {
                DeletionResult imageDeletionResult = await _cloudinaryManager.DestoryImage(_category.Image);
                if (imageDeletionResult.Result.Equals("ok"))
                {
                    imageUrl = await _cloudinaryManager.UploadImage(category.Image, "category-images");
                }
            }

            var updatedcategory = new Category
            {
                Id = category.Id,
                Image = imageUrl
            };


            try
            {
                var updatedCategoryBool = await _categoryService.UpdateCategory(updatedcategory, translations, languageIds);
                return Ok(updatedCategoryBool);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/translations")]
        public ActionResult<CategoryDTO> GetCategoryWithAllTranslations(int id)
        {
            var categoryDto = _categoryService.GetCategoryWithAllTranslations(id);
            if (categoryDto == null)
            {
                return NotFound();
            }

            return Ok(categoryDto);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async void Delete(int id)
        {
            var deletedCategory = _categoryService.GetCategoryById(id, 1);
             DeletionResult deletionResult= await _cloudinaryManager.DestoryImage(deletedCategory.Image);
            if (deletionResult.Result.Equals("ok"))
            {
                _categoryService.DeleteCategory(id);
            }
        }


    }
}
