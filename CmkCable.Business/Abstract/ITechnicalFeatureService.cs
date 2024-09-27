using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface ITechnicalFeatureService
    {
        List<TechnicalFeature> GetAllFeaturesByProductId(int productId);
        List<TechnicalFeature> GetAllFeatures();
        TechnicalFeature CreateTechnicalFeature(TechnicalFeature technicalFeature);
        TechnicalFeature UpdateTechnicalFeature(TechnicalFeature technicalFeature);
        void DeleteTechnicalFeature(int technicalFeatureId);

    }
}
