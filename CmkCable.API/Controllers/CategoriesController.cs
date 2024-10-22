using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
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
        public CategoriesController()
        {
            _categoryService = new CategoryManager();
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
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDTO categoryDto,  [FromForm] List<string> translations, [FromForm] List<int> languageIds)
        {
            if (categoryDto == null || translations == null || languageIds == null)
            {
                return BadRequest("Invalid input data.");
            }

            if (translations.Count != languageIds.Count)
            {
                return BadRequest("The number of translations must match the number of languages.");
            }

            try
            {

                await _categoryService.CreateCategory(categoryDto, translations, languageIds);

                return CreatedAtAction(nameof(CreateCategory), new { id = categoryDto.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory(
     [FromForm] Category category,
     [FromForm] List<string> translations,
     [FromForm] List<int> languageIds)
        {
            if (category.Id <= 0)
            {
                return BadRequest("Category ID is required.");
            }

            if (translations == null || languageIds == null || translations.Count != languageIds.Count)
            {
                return BadRequest("Translations and language IDs must have the same length.");
            }

            try
            {
                var updatedCategory = await _categoryService.UpdateCategory(category, translations, languageIds);
                return Ok(updatedCategory);
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
        public void Delete(int id) { _categoryService.DeleteCategory(id); }


    }
}
