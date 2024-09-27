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
    public class MainSwiperItemsController : ControllerBase
    {

        private IMainPageSwiperItemService _mainPageSwiperItemService;
        public MainSwiperItemsController() { _mainPageSwiperItemService = new MainPageSwiperItemManager(); }

        [HttpGet]
        public List<MainPageSwiperItem> Get()
        {
            return _mainPageSwiperItemService.GetAlMainPageSwiperItems();
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateItem([FromForm] MainPageSwiperItem _mainPageSwiperItem)
        {
            if (_mainPageSwiperItem.Video == null || _mainPageSwiperItem.Video.Length == 0)
                return BadRequest("No video uploaded.");

            byte[] videoBytes;

            using (var memoryStream = new MemoryStream())
            {
                await _mainPageSwiperItem.Video.CopyToAsync(memoryStream);
                videoBytes = memoryStream.ToArray();
            }

            if (videoBytes.Length > 700 * 1024 * 1024) 
                return BadRequest("Video size exceeds the limit.");

            var mainSwiperItem = new MainPageSwiperItem
            {
                Description = _mainPageSwiperItem.Description,
                VideoData = videoBytes,
            };

            var createdItem =  _mainPageSwiperItemService.CreateMainPageSwiperItem(mainSwiperItem);
            return Ok(createdItem);
        }



        [HttpPut("update")] 
        public MainPageSwiperItem UpdateMainSwiperItem(MainPageSwiperItem mainSwiperItem)
        {
            return _mainPageSwiperItemService.UpdateMainPageSwiperItem(mainSwiperItem);
        }

    }
}
