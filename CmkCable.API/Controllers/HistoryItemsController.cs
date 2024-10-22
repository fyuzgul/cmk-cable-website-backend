using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryItemsController : ControllerBase
    {
        private IHistoryItemService _historyItemService;
        public HistoryItemsController() { _historyItemService = new HistoryItemManager(); }

        [HttpGet]
        public List<HistoryItem> GetAllHistoryItems() { return _historyItemService.GetAllHistoryItems(); }
        [HttpPost("create")]
        public async Task<IActionResult> CreateHistoryItem([FromForm] HistoryItem _historyItem)
        {
            if (_historyItem.Image == null || _historyItem.Image.Length == 0)
                return BadRequest("No Image uploaded.");
            byte[] imageBytes;

            using (var memoryStream = new MemoryStream())
            {
                await _historyItem.Image.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }
            var historyItem = new HistoryItem
            {
                Year = _historyItem.Year,
                Title = _historyItem.Title,
                Description =  _historyItem.Description,
                ImageData = imageBytes,
            };
            var createdItem = _historyItemService.CreateHistoryItem(historyItem);
            return Ok(createdItem);
        }
        [HttpGet("{id}")]
        public HistoryItem GetHistoryItem(int id) { return _historyItemService.GetHistoryItem(id); }
        [HttpDelete("delete")]
        public void DeleteHistoryItem(int id) { _historyItemService.DeleteHistoryItem(id); }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateHistoryItem([FromForm]HistoryItem _historyItem) {
            if (_historyItem.Id <= 0)
            {
                return BadRequest("Product ID is required.");
            }

            var existingItem = _historyItemService.GetHistoryItem(_historyItem.Id);

            if (existingItem == null)
            {
                return NotFound($"Product with ID {_historyItem.Id} not found.");
            }

            if (_historyItem.Image != null && _historyItem.Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await _historyItem.Image.CopyToAsync(memoryStream);
                    existingItem.ImageData = memoryStream.ToArray();
                }
            }

            existingItem.Description = string.IsNullOrEmpty(_historyItem.Description) ? existingItem.Description : _historyItem.Description;
            existingItem.Title = string.IsNullOrEmpty(_historyItem.Title) ? existingItem.Title : _historyItem.Title;
            existingItem.Year = string.IsNullOrEmpty(_historyItem.Year) ? existingItem.Year : _historyItem.Year;
            var updatedItem = _historyItemService.UpdateHistoryItem(existingItem);

            return Ok(updatedItem);
        }
    }
}
