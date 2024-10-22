using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class HistoryItemRepository : IHistoryItemRepository
    {
        public HistoryItem CreateHistoryItem(HistoryItem historyItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.HistoryItems.Add(historyItem);
                cmkCableDbContext.SaveChanges();
                return historyItem;
            }
        }

        public void DeleteHistoryItem(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedItem = cmkCableDbContext.HistoryItems.Find(id);
                cmkCableDbContext.HistoryItems.Remove(deletedItem);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<HistoryItem> GetAllHistoryItems()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.HistoryItems.ToList();
            }
        }

        public HistoryItem GetHistoryItem(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.HistoryItems.Find(id);
            }
        }

        public HistoryItem UpdateHistoryItem(HistoryItem historyItem)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.HistoryItems.Update(historyItem);
                cmkCableDbContext.SaveChanges();
                return historyItem;
            }
        }
    }
}
