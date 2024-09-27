using CmkCable.DataAccess.Abstract;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmkCable.DataAccess.Concrete
{
    public class TechnicalFeatureRepository : ITechnicalFeatureRepository
    {
        public TechnicalFeature CreateTechnicalFeature(TechnicalFeature technicalFeature)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.Add(technicalFeature);    
                cmkCableDbContext.SaveChanges();
                return technicalFeature;
            }
        }

        public void DeleteTechnicalFeature(int technicalFeatureId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                var deletedTechnicalFeature = cmkCableDbContext.TechnicalFeatures.Find(technicalFeatureId);
                cmkCableDbContext.TechnicalFeatures.Remove(deletedTechnicalFeature);
                cmkCableDbContext.SaveChanges() ;
            }
        }

        public List<TechnicalFeature> GetAllFeatures()
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.TechnicalFeatures.ToList();
            }
        }

        public List<TechnicalFeature> GetAllFeaturesByProductId(int productId)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                return cmkCableDbContext.TechnicalFeatures.Where(p => p.ProductId == productId).ToList();
            }
        }

        public TechnicalFeature UpdateTechnicalFeature(TechnicalFeature technicalFeature)
        {
            using (var cmkCableDbContext = new CmkCableDbContext())
            {
                cmkCableDbContext.TechnicalFeatures.Update(technicalFeature);
                cmkCableDbContext.SaveChanges();
                return technicalFeature;
            }
        }
    }
}
