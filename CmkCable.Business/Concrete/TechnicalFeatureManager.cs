using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class TechnicalFeatureManager : ITechnicalFeatureService
    {
        private ITechnicalFeatureRepository _technicalFeatureRepository;
        public TechnicalFeatureManager() { _technicalFeatureRepository = new TechnicalFeatureRepository(); }

        public TechnicalFeature CreateTechnicalFeature(TechnicalFeature technicalFeature)
        {
            return _technicalFeatureRepository.CreateTechnicalFeature(technicalFeature);
        }

        public void DeleteTechnicalFeature(int technicalFeatureId)
        {
            _technicalFeatureRepository.DeleteTechnicalFeature(technicalFeatureId);
        }

        public List<TechnicalFeature> GetAllFeatures()
        {
           return _technicalFeatureRepository.GetAllFeatures();   
        }

        public List<TechnicalFeature> GetAllFeaturesByProductId(int productId)
        {
            return _technicalFeatureRepository.GetAllFeaturesByProductId(productId);
        }

        public TechnicalFeature UpdateTechnicalFeature(TechnicalFeature technicalFeature)
        {
            return _technicalFeatureRepository.UpdateTechnicalFeature(technicalFeature);
        }
    }
}
