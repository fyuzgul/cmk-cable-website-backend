using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
using DTOs.CreateDTOs;
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
        [HttpPost("create")]
        public HistoryItem CreateHistoryItem([FromForm] CreateHistoryItemDTO historyItemDTO, [FromForm] List<string> titles, [FromForm]List<string> descriptions, [FromForm] List<int> languageIds)
        {
            return _historyItemService.CreateHistoryItem(historyItemDTO, titles, descriptions, languageIds);
        }

        [HttpDelete("delete/{id}")]
        public void DeleteHistoryItem(int id) { _historyItemService.DeleteHistoryItem(id); }

        [HttpGet]
        public List<HistoryItemDTO> GetAllHistoryItems() { return _historyItemService.GetAllHistoryItems(); }

        [HttpGet("{id}")]
        public List<HistoryItemDTO> GetHistoryItem(int id) { return _historyItemService.GetAllHistoryItemWithSelectedLanguage(id); }

        [HttpPut("update")]
        public HistoryItem UpdateHistoryItem([FromForm] HistoryItem historyItem, [FromForm] List<string> titles, [FromForm] List<string> descriptions, [FromForm] List<int> languageIds)
        {
            return _historyItemService.UpdateHistoryItem(historyItem, titles, descriptions, languageIds);
        }

    }
}
