using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class MainPageSwiperItemManager : IMainPageSwiperItemService
    {
        private IMainPageSwiperItemRepository _mainPageSwiperItemRepository;

        public MainPageSwiperItemManager() { _mainPageSwiperItemRepository = new MainSwiperItemRepository(); }
        public MainPageSwiperItem CreateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem)
        {
           return _mainPageSwiperItemRepository.CreateMainPageSwiperItem(mainPageSwiperItem);
        }

        public void DeleteMainPageSwiperItem(int id)
        {
            _mainPageSwiperItemRepository.DeleteMainPageSwiperItem(id);
        }

        public List<MainPageSwiperItem> GetAlMainPageSwiperItems()
        {
            return _mainPageSwiperItemRepository.GetAlMainPageSwiperItems();
        }

        public MainPageSwiperItem GetMainPageSwiperItemById(int id)
        {
            return _mainPageSwiperItemRepository.GetMainPageSwiperItemById(id);
        }

        public MainPageSwiperItem UpdateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem)
        {
            return _mainPageSwiperItemRepository.UpdateMainPageSwiperItem(mainPageSwiperItem);
        }
    }
}
