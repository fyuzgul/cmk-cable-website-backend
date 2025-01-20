

using CmkCable.Entities;
using System.Collections.Generic;

namespace CmkCable.Business.Abstract
{
    public interface ISwiperItemService
    {
        SwiperItem CreateSwiperItem(SwiperItem swiperItem);
        void DeleteSwiperItem(int id);
        List<SwiperItem> GetAllSwiperItems();
        SwiperItem GetSwiperItemById(int id);
        SwiperItem UpdateSwiperItem(SwiperItem swiperItem);
    }
}
