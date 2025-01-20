using CmkCable.Entities;
using DTOs.CreateDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmkCable.DataAccess.Abstract
{
    public interface ISwiperItemRepository
    {
        List<SwiperItem> GetAllSwiperItems();
        SwiperItem GetSwiperItemById(int id);
        SwiperItem CreateSwiperItem(SwiperItem swiperItem);
        SwiperItem UpdateSwiperItem(SwiperItem swiperItem);
        void DeleteSwiperItem(int id);
    }
}
