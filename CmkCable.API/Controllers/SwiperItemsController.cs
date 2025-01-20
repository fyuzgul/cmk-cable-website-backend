using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using DTOs.CreateDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwiperItemsController : ControllerBase
    {
        private ISwiperItemService _swiperItemService;

        public SwiperItemsController()
        {
            _swiperItemService = new SwiperItemManager();
        }

        [HttpPost("create")]
        public SwiperItem Create([FromBody] SwiperItem item)
        {
            return _swiperItemService.CreateSwiperItem(item);
        }

        [HttpGet]
        public List<SwiperItem> GetAllSwiperItems() { return _swiperItemService.GetAllSwiperItems(); }

        [HttpGet("{id}")]
        public SwiperItem GetSwiperItemById(int id) {return _swiperItemService.GetSwiperItemById(id);}

        [HttpDelete("delete/{id}")]
        public void DeleteSwiperItemById(int id) { _swiperItemService.DeleteSwiperItem(id);}

        [HttpPut("update")]
        public SwiperItem UpdateSwiperItem(SwiperItem swiperItem) { return _swiperItemService.UpdateSwiperItem(swiperItem); }

    }
}
