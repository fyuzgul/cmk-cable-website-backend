using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        public CategoriesController() {
            _categoryService = new CategoryManager();
        }
        [HttpGet]
        public List<Category> Get()
        {
            return _categoryService.GetAllCategories();
        }

        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return _categoryService.GetCategoryById(id);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromForm] Category _category)
        {
            if (_category.Image == null || _category.Image.Length == 0)
                return BadRequest("No Image uploaded.");
            byte[] imageBytes;

            using (var memoryStream = new MemoryStream())
            {
                await _category.Image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            var category = new Category
            {
                Name = _category.Name,
                ImageData = imageBytes,
            };
            var createdCategory = _categoryService.CreateCategory(category);
            return Ok(createdCategory);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateCategory([FromForm] Category updateCategory)
        {
            if (updateCategory.Id <= 0)
            {
                return BadRequest("Product ID is required.");
            }

            var existingCategory = _categoryService.GetCategoryById(updateCategory.Id);

            if (existingCategory == null)
            {
                return NotFound($"Product with ID {updateCategory.Id} not found.");
            }

            if (updateCategory.Image != null && updateCategory.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await updateCategory.Image.CopyToAsync(memoryStream);
                    existingCategory.ImageData = memoryStream.ToArray();
                }
            }

            existingCategory.Name = string.IsNullOrEmpty(updateCategory.Name) ? existingCategory.Name : updateCategory.Name;
            // Güncelleme işlemi
            var updatepCate = _categoryService.UpdateCategory(existingCategory);

            return Ok(updatepCate);
        }

        [HttpDelete("delete/{id}")]
        public void Delete(int id) { _categoryService.DeleteCategory(id); }
      

    }
}
