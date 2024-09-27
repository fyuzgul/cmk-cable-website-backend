using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class CategoryRepository : ICategoryRepository
    {
        public Category CreateCategory(Category category)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Categories.Add(category);
                cmkCableDbContext.SaveChanges();
                return category;
            }
        }

        public void DeleteCategory(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedCategory = cmkCableDbContext.Categories.Find(id);
                cmkCableDbContext.Categories.Remove(deletedCategory);
                cmkCableDbContext.SaveChanges();
            }
        }

        public List<Category> GetAllCategories()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Categories.ToList();
            }
        }

        public Category GetCategoryById(int id)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.Categories.Find(id);
            }
        }

        public Category UpdateCategory(Category category)
        {
            using(var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Categories.Update(category);
                cmkCableDbContext.SaveChanges();
                return category;
            }
        }
    }
}
