using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.Business.Concrete
{
    public class SwiperItemManager : ISwiperItemService
    {
        private ISwiperItemRepository _swiperItemRepository;

        public SwiperItemManager()
        {
            _swiperItemRepository = new SwiperItemRepository();
        }
        public SwiperItem CreateSwiperItem(SwiperItem swiperItem)
        {
            return _swiperItemRepository.CreateSwiperItem(swiperItem);  
        }

        public void DeleteSwiperItem(int id)
        {
            _swiperItemRepository.DeleteSwiperItem(id);
        }

        public List<SwiperItem> GetAllSwiperItems()
        {
            return _swiperItemRepository.GetAllSwiperItems();
        }

        public SwiperItem GetSwiperItemById(int id)
        {
            return _swiperItemRepository.GetSwiperItemById(id);
        }

        public SwiperItem UpdateSwiperItem(SwiperItem swiperItem)
        {
            return _swiperItemRepository.UpdateSwiperItem(swiperItem);
        }
    }
}
