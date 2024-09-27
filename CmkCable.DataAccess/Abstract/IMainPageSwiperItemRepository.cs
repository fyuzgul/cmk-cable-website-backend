using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.DataAccess.Abstract
{
    public interface IMainPageSwiperItemRepository
    {
        List<MainPageSwiperItem> GetAlMainPageSwiperItems();
        MainPageSwiperItem CreateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem);
        MainPageSwiperItem GetMainPageSwiperItemById(int id);

        MainPageSwiperItem UpdateMainPageSwiperItem(MainPageSwiperItem mainPageSwiperItem);

        void DeleteMainPageSwiperItem(int id);
    }
}
