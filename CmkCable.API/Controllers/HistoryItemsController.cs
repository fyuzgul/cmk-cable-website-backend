using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs;
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

       
        [HttpGet("{id}")]
        public List<HistoryItemDTO> GetHistoryItem(int id) { return _historyItemService.GetAllHistoryItemWithSelectedLanguage(id); }
       
    }
}
