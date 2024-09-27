using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IMainPageSwiperItemService
    {
        List<MainPageSwiperItem> GetAlMainPageSwiperItems();
        MainPageSwiperItem CreateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem);

        MainPageSwiperItem UpdateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem);
        MainPageSwiperItem GetMainPageSwiperItemById(int id);
        void DeleteMainPageSwiperItem(int id);
    }
}
